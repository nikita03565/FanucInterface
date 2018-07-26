using System;

public static class Poly34
{

    const double eps = 1e-14;

    public static double _root3(double x)
    {
        double s = 1.0;
        while (x < 1.0)
        {
            x *= 8.0;
            s *= 0.5;
        }
        while (x > 8.0)
        {
            x *= 0.125;
            s *= 2.0;
        }
        double r = 1.5;
        r -= 1.0 / 3.0 * (r - x / (r * r));
        r -= 1.0 / 3.0 * (r - x / (r * r));
        r -= 1.0 / 3.0 * (r - x / (r * r));
        r -= 1.0 / 3.0 * (r - x / (r * r));
        r -= 1.0 / 3.0 * (r - x / (r * r));
        r -= 1.0 / 3.0 * (r - x / (r * r));
        return r * s;
    }

    public static double root3(double x)
    {
        if (x > 0)
            return _root3(x);
        if (x < 0)
            return -_root3(-x);
        return 0.0;
    }


    // x - array of size 2
    // return 2: 2 real roots x[0], x[1]
    // return 0: pair of complex roots: x[0]±i*x[1]
    public static int SolveP2(ref double[] x, double a, double b) // solve equation x^2 + a*x + b = 0
    {
        double D = 0.25 * a * a - b;
        if (D >= 0)
        {
            D = Math.Sqrt(D);
            x[0] = 0.5 * a + D;
            x[1] = 0.5 * a - D;
            return 2;
        }
        x[0] = 0.5 * a;
        x[1] = Math.Sqrt(-D);
        return 0;
    }

    //---------------------------------------------------------------------------
    // x - array of size 3
    // In case 3 real roots: => x[0], x[1], x[2], return 3
    //         2 real roots: x[0], x[1],          return 2
    //         1 real root : x[0], x[1] ± i*x[2], return 1
    public static int SolveP3(ref double[] x, double a, double b, double c) // solve cubic equation x^3 + a*x^2 + b*x + c = 0
    {
        double a2 = a * a;
        double q = (a2 - 3 * b) / 9;
        double r = (a * (2 * a2 - 9 * b) + 27 * c) / 54;
        // equation x^3 + q*x + r = 0
        double r2 = r * r;
        double q3 = q * q * q;
        double A, B;
        if (r2 < q3)
        {
            double t = r / Math.Sqrt(q3);
            if (t < -1)
                t = -1;
            if (t > 1)
                t = 1;
            t = Math.Acos(t);
            a /= 3;
            q = -2 * Math.Sqrt(q);
            x[0] = q * Math.Cos(t / 3) - a;
            x[1] = q * Math.Cos((t + 2 * Math.PI) / 3) - a;
            x[2] = q * Math.Cos((t - 2 * Math.PI) / 3) - a;
            return (3);
        }
        //A =-pow(Math.Abs(r)+Math.Sqrt(r2-q3),1./3); 
        A = -root3(Math.Abs(r) + Math.Sqrt(r2 - q3));
        if (r < 0)
            A = -A;
        B = A == 0 ? 0 : B = q / A;

        a /= 3;
        x[0] = (A + B) - a;
        x[1] = -0.5 * (A + B) - a;
        x[2] = 0.5 * Math.Sqrt(3.0) * (A - B);
        if (Math.Abs(x[2]) < eps)
        {
            x[2] = x[1];
            return (2);
        }
        return (1);
    }// SolveP3(double *x,double a,double b,double c) {	
     //---------------------------------------------------------------------------
     // a>=0!
    public static void CSqrt(double x, double y, ref double a, ref double b) // returns:  a+i*s = Math.Sqrt(x+i*y)
    {
        double r = Math.Sqrt(x * x + y * y);
        if (y == 0)
        {
            r = Math.Sqrt(r);
            if (x >= 0)
            {
                a = r;
                b = 0;
            }
            else
            {
                a = 0;
                b = r;
            }
        }
        else
        {
            // y != 0
            a = Math.Sqrt(0.5 * (x + r));
            b = 0.5 * y / a;
        }
    }

    //---------------------------------------------------------------------------
    public static int SolveP4Bi(ref double[] x, double b, double d) // solve equation x^4 + b*x^2 + d = 0
    {
        double D = b * b - 4 * d;
        if (D >= 0)
        {
            double sx1;
            double sx2;
            double sD = Math.Sqrt(D);
            double x1 = (-b + sD) / 2;
            double x2 = (-b - sD) / 2; // x2 <= x1
            if (x2 >= 0) // 0 <= x2 <= x1, 4 real roots
            {
                sx1 = Math.Sqrt(x1);
                sx2 = Math.Sqrt(x2);
                x[0] = -sx1;
                x[1] = sx1;
                x[2] = -sx2;
                x[3] = sx2;
                return 4;
            }
            if (x1 < 0) // x2 <= x1 < 0, two pair of imaginary roots
            {
                sx1 = Math.Sqrt(-x1);
                sx2 = Math.Sqrt(-x2);
                x[0] = 0;
                x[1] = sx1;
                x[2] = 0;
                x[3] = sx2;
                return 0;
            }
            // now x2 < 0 <= x1 , two real roots and one pair of imginary root
            sx1 = Math.Sqrt(x1);
            sx2 = Math.Sqrt(-x2);
            x[0] = -sx1;
            x[1] = sx1;
            x[2] = 0;
            x[3] = sx2;
            return 2;
        }
        // if( D < 0 ), two pair of compex roots
        double sD2 = 0.5 * Math.Sqrt(-D);
        CSqrt(-0.5 * b, sD2, ref x[0], ref x[1]);
        CSqrt(-0.5 * b, -sD2, ref x[2], ref x[3]);
        return 0;
        // if( D>=0 ) 
    } // SolveP4Bi(double *x, double b, double d)	// solve equation x^4 + b*x^2 d
      //---------------------------------------------------------------------------

    static void dblSort3(ref double a, ref double b, ref double c) // make: a <= b <= c
    {
        double t;
        if (a > b)
        { t = b; b = a; a = t; }
        // SWAP(a, b); // now a<=b
        if (c < b)
        {
            t = c; c = b; b = t;
            //SWAP(b, c); // now a<=b, b<=c
            if (a > b)
            { t = b; b = a; a = t; }
            //SWAP(a, b);// now a<=b
        }
    }

    //---------------------------------------------------------------------------
    public static int SolveP4De(ref double[] x, double b, double c, double d) // solve equation x^4 + b*x^2 + c*x + d
    {
        //if( c==0 ) return SolveP4Bi(x,b,d); // After that, c!=0
        if (Math.Abs(c) < 1e-14 * (Math.Abs(b) + Math.Abs(d)))
            return SolveP4Bi(ref x, b, d); // After that, c!=0

        int res3 = SolveP3(ref x, 2 * b, b * b - 4 * d, -c * c); // solve resolvent
                                                                 // by Viet theorem:  x1*x2*x3=-c*c not equals to 0, so x1!=0, x2!=0, x3!=0
        double sz1;
        double sz2;
        double sz3;
        if (res3 > 1) // 3 real roots, 
        {
            dblSort3(ref x[0], ref x[1], ref x[2]); // sort roots to x[0] <= x[1] <= x[2]
                                        // Note: x[0]*x[1]*x[2]= c*c > 0
            if (x[0] > 0) // all roots are positive
            {
                sz1 = Math.Sqrt(x[0]);
                sz2 = Math.Sqrt(x[1]);
                sz3 = Math.Sqrt(x[2]);
                // Note: sz1*sz2*sz3= -c (and not equal to 0)
                if (c > 0)
                {
                    x[0] = (-sz1 - sz2 - sz3) / 2;
                    x[1] = (-sz1 + sz2 + sz3) / 2;
                    x[2] = (+sz1 - sz2 + sz3) / 2;
                    x[3] = (+sz1 + sz2 - sz3) / 2;
                    return 4;
                }
                // now: c<0
                x[0] = (-sz1 - sz2 + sz3) / 2;
                x[1] = (-sz1 + sz2 - sz3) / 2;
                x[2] = (+sz1 - sz2 - sz3) / 2;
                x[3] = (+sz1 + sz2 + sz3) / 2;
                return 4;
            } // if( x[0] > 0) // all roots are positive
              // now x[0] <= x[1] < 0, x[2] > 0
              // two pair of comlex roots
            sz1 = Math.Sqrt(-x[0]);
            sz2 = Math.Sqrt(-x[1]);
            sz3 = Math.Sqrt(x[2]);

            if (c > 0) // sign = -1
            {
                x[0] = -sz3 / 2;
                x[1] = (sz1 - sz2) / 2; // x[0]±i*x[1]
                x[2] = sz3 / 2;
                x[3] = (-sz1 - sz2) / 2; // x[2]±i*x[3]
                return 0;
            }
            // now: c<0 , sign = +1
            x[0] = sz3 / 2;
            x[1] = (-sz1 + sz2) / 2;
            x[2] = -sz3 / 2;
            x[3] = (sz1 + sz2) / 2;
            return 0;
        } // if( res3>1 )	// 3 real roots, 
          // now resoventa have 1 real and pair of compex roots
          // x[0] - real root, and x[0]>0, 
          // x[1]±i*x[2] - complex roots, 
          // x[0] must be >=0.0 But one times x[0]=~ 1e-17, so:
        if (x[0] < 0)
            x[0] = 0;
        sz1 = Math.Sqrt(x[0]);
        double szr = 0, szi = 0;
        CSqrt(x[1], x[2], ref szr, ref szi); // (szr+i*szi)^2 = x[1]+i*x[2]
        if (c > 0) // sign = -1
        {
            x[0] = -sz1 / 2 - szr; // 1st real root
            x[1] = -sz1 / 2 + szr; // 2nd real root
            x[2] = sz1 / 2;
            x[3] = szi;
            return 2;
        }
        // now: c<0 , sign = +1
        x[0] = sz1 / 2 - szr; // 1st real root
        x[1] = sz1 / 2 + szr; // 2nd real root
        x[2] = -sz1 / 2;
        x[3] = szi;
        return 2;
    } // SolveP4De(double *x, double b, double c, double d)	// solve equation x^4 + b*x^2 + c*x + d
      //-----------------------------------------------------------------------------
    public static double N4Step(double x, double a, double b, double c, double d) // one Newton step for x^4 + a*x^3 + b*x^2 + c*x + d
    {
        double fxs = ((4 * x + 3 * a) * x + 2 * b) * x + c; // f'(x)
        if (fxs == 0)
            return 1e99;
        double fx = (((x + a) * x + b) * x + c) * x + d; // f(x)
        return x - fx / fxs;
    }

    //-----------------------------------------------------------------------------
    // x - array of size 4
    // return 4: 4 real roots x[0], x[1], x[2], x[3], possible multiple roots
    // return 2: 2 real roots x[0], x[1] and complex x[2]±i*x[3], 
    // return 0: two pair of complex roots: x[0]±i*x[1],  x[2]±i*x[3], 
    public static int SolveP4(ref float[] x, float a, float b, float c, float d)
    {
        double[] x1 = new double[4] { (double)x[0], (double)x[1], (double)x[2], (double)x[3] };
        // solve equation x^4 + a*x^3 + b*x^2 + c*x + d by Dekart-Euler method
        // move to a=0:
        double d1 = d + 0.25 * a * (0.25 * b * a - 3.0 / 64 * a * a * a - c);
        double c1 = c + 0.5 * a * (0.25 * a * a - b);
        double b1 = b - 0.375 * a * a;
        int res = SolveP4De(ref x1, b1, c1, d1);
        if (res == 4)
        {
            x1[0] -= a / 4.0;
            x1[1] -= a / 4.0;
            x1[2] -= a / 4.0;
            x1[3] -= a / 4.0;
        }
        else if (res == 2)
        {
            x1[0] -= a / 4.0;
            x1[1] -= a / 4.0;
            x1[2] -= a / 4.0;
        }
        else
        {
            x1[0] -= a / 4.0;
            x1[2] -= a / 4.0;
        }
        // one Newton step for each real root:
        if (res > 0)
        {
            x1[0] = (float)N4Step(x1[0], a, b, c, d);
            x1[1] = (float)N4Step(x1[1], a, b, c, d);
        }
        if (res > 2)
        {
            x1[2] = (float)N4Step(x1[2], a, b, c, d);
            x1[3] = (float)N4Step(x1[3], a, b, c, d);
        }
        x[0] = (float)x1[0];
        x[1] = (float)x1[1];
        x[2] = (float)x1[2];
        x[3] = (float)x1[3];
        return res;
    }

    //-----------------------------------------------------------------------------
}
