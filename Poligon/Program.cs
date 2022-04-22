using System.Drawing;
//variables
#region
int N;
int K;
List<Point> points = new List<Point>();
Random rnd = new Random();
#endregion
//helper functions
#region
double distanceFromLine(Point lp1, Point lp2, Point p)
{
    double distance = ((lp2.Y - lp1.Y) * p.X - (lp2.X - lp1.X) * p.Y + lp2.X * lp1.Y - lp2.Y * lp1.X) / Math.Sqrt(Math.Pow(lp2.Y - lp1.Y, 2) + Math.Pow(lp2.X - lp1.X, 2));
    return distance;
}

double outerDistanceToBoundary(List<Point> solution)
{
    double sum_min_distance = 0;

    for (int i = 0; i < points.Count; i++)
    {
        double min_dist = 0;
        for (int j = 0; j < solution.Count; j++)
        {
            double act_dist = distanceFromLine(solution[j], solution[(j + 1) % solution.Count], points[i]);
            if (i == 0 || act_dist < min_dist)
                min_dist = act_dist;
        }
        if (min_dist < 0)
            sum_min_distance += -min_dist;
    }

    return sum_min_distance;
}

double LengthOfBoundary(List<Point> solution)
{
    double sum_length = 0;
    for (int i = 0; i < solution.Count; i++)
    {
        var p1 = solution[i];
        var p2 = solution[(i + 1) % solution.Count];
        sum_length += Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
    return sum_length;
}

double objective(List<Point> solution)
{
    return LengthOfBoundary(solution);
}

double constraint(List<Point> solution)
{
    return -outerDistanceToBoundary(solution);
}

List<Point> fitness(List<Point> solution1, List<Point> solution2)
{
    if (LengthOfBoundary(solution1) < LengthOfBoundary(solution2))
        return solution1;
    else
        return solution2;
}

void generatePoints()
{
    for (int i = 0; i < 50; i++)
    {
        points.Add(new Point(rnd.Next(0, 101), rnd.Next(0, 101)));
    }
}
#endregion

//HillClimbSteepestAscent
#region
List<Point> HillClimb(List<Point> search, int StopCondition, int epsilon)
{
    List<Point> P = new List<Point>();
    for (int i = 0; i < 4; i++)
    {
        P.Add(new Point(rnd.Next(0, 101), rnd.Next(0, 101)));
    }
    bool stuck = false;
    int step = 0;
    while (step < StopCondition && !stuck)
    {
        List<Point> Q = P;
        var randQPoint = Q[rnd.Next(0, 4)];
        Q.Remove(randQPoint);
        randQPoint.X += epsilon;
        randQPoint.Y += epsilon;
        Q.Add(randQPoint);
        var F = fitness(Q, P);
        if (F.Equals(Q))
        {
            P = Q;
        }
        else
        {
            stuck = false;
        }
        step++;
    }
    return P;
}
#endregion

//program
#region
generatePoints();
Console.WriteLine(LengthOfBoundary(HillClimb(points, 100, 1)));
#endregion