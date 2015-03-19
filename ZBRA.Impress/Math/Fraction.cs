using System;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace ZBRA.Impress.Math
{
    /// <summary>
    /// Represents a rational number that is not , necessarily, an integer.  Examples 2/3 , 8/5, 9/1, 0/1
    /// Integers are not closed for division, in fact no division operation exists that taken two integers always produces another integer.
    /// There is an Integer Division operation that taking two integers produces another two integers know as quocient and remaider.
    /// To expand the concept of division, in a closed form, the rational numbers (fractions) are introduced.  All four basic arithmetic operations are closed for racional numbers.
    /// 
    /// Rational numbers allow for long calculations with no loss of precision that is not possible with double , float or even decimal.
    /// 
    /// Using doubles a for loop that suns 0.1 ten times does not result in 1. Using Fraction it does.
    /// 
    /// 
    /// Internally this implementation uses BigInteger to retain the numerator and denominator values. 
    /// After each calculation the fraction is simplified so the numerator and denominator are always coprimes.
    /// </summary>
    public struct Fraction : IComparable<Fraction>
    {

        public static readonly Fraction Zero = new Fraction(0, 1);
        public static readonly Fraction One = new Fraction(1, 1);

        private BigInteger numerator;
        private BigInteger denominator;

        public static Fraction ValueOf(long number)
        {
            return new Fraction(number, 1);
        }

        public static Fraction ValueOf(decimal number)
        {
            if (number.IsInteger())
            {
                // the decimal is really an integer
                return new Fraction(new BigInteger(number), 1);
            }

            return FromDecimalStringLiteral(new DecimalConverter().ConvertToInvariantString(number));
        }

        public static Fraction ValueOf(double number)
        {
            if (number.IsInteger())
            {
                // the double is really an integer
                return new Fraction(new BigInteger(number), 1);
            }
            else
            {
                return FromDecimalStringLiteral(new DoubleConverter().ConvertToInvariantString(number));
            }
        }

        private static Fraction FromDecimalStringLiteral(string str)
        {
            int pos = str.IndexOf('E');
            if (pos < 0)
            {
                pos = str.IndexOf('.');
                if (pos < 0)
                {
                    return new Fraction(BigInteger.Parse(str), 1);
                }
                else
                {
                    var builder = new StringBuilder(str.Replace(".", ""));
                    while (builder[0] == '0')
                    {
                        builder.Remove(0, 1);
                    }

                    var expo = str.Length - pos - 1;
                    var exponent = BigInteger.Pow(new BigInteger(10), expo);

                    return new Fraction(BigInteger.Parse(builder.ToString()), exponent);
                }
            }
            else
            {
                var sign = str.Substring(pos + 1, 1).Equals("+") ? 1 : -1;
                if (sign != -1)
                {
                    throw new ArgumentException("Only negative exponent representation is supported");
                }
                var exponent = int.Parse(str.Substring(pos + 2));

                var expower = BigInteger.Pow(new BigInteger(10), exponent);
                str = str.Substring(0, pos).Replace(".", "");

                return new Fraction(BigInteger.Parse(str), expower);
            }

        }

        public static Fraction ValueOf(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                throw new ArithmeticException("Cannot create a fraction with denominator zero");
            }
            else if (numerator == 0)
            {
                return Zero;
            }
            else if (numerator == denominator)
            {
                return One;
            }
            return new Fraction(numerator, denominator);
        }

        private Fraction(BigInteger numerator, BigInteger denominator)
        {

            if (numerator == 0)  // all forms of zero are alike.
            {
                denominator = 1;
            }
            else if (numerator == denominator)
            { // all forms of one are alike.
                numerator = 1;
                denominator = 1;
            }
            else if (denominator != 1)
            {
                var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
                numerator /= gcd;
                denominator /= gcd;
            }

            // if negative , change sign in denominator
            if (denominator.Sign < 0)
            {
                //move negative sign to numerator
                numerator = -numerator;
                denominator = -denominator;
            }

            this.numerator = numerator;
            this.denominator = denominator;

        }

        public bool IsZero()
        {
            return this.numerator == 0 || this.denominator == 0;
        }

        public bool IsOne()
        {
            return this.numerator.CompareTo(this.denominator) == 0;
        }

        public Fraction Times(Fraction other)
        {
            return Multiply(this, other);
        }

        public Fraction Times(decimal other)
        {
            return Multiply(this, Fraction.ValueOf(other));
        }

        public Fraction Negate()
        {
            return new Fraction(-this.numerator, this.denominator);
        }

        /// <summary>
        ///  Inverts the fraction. Fraction a/b will be b/a. 
        ///  An ArithmeticException will be throwned if fraction.IsZero is true.
        /// </summary>
        /// <returns></returns>
        public Fraction Invert()
        {
            if (this.IsZero())
            {
                throw new ArithmeticException("Cannot invert zero");
            }
            return new Fraction(this.denominator, this.numerator);
        }

        private static BigInteger LCM(BigInteger left, BigInteger right)
        {
            // LCM = |a.b| / GCD(a,b) = (|a| / GCD(a,b)) . |b|
            // https://en.wikipedia.org/wiki/Least_common_multiple

            if (left.Sign < 0)
            {
                left = -left;
            }
            if (right.Sign < 0)
            {
                right = -right;
            }
            return (left / BigInteger.GreatestCommonDivisor(left, right)) * right;

        }

        #region Operators

        public static bool operator ==(Fraction left, Fraction right)
        {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(Fraction left, Fraction right)
        {
            return left.CompareTo(right) != 0;
        }

        public static Fraction operator *(Fraction left, Fraction right)
        {
            return Multiply(left, right);
        }

        public static Fraction operator *(Fraction left, int right)
        {
            return MultiplyInt(left, right);
        }

        private static Fraction MultiplyInt(Fraction left, int right)
        {
            // r = a/b * n = (a*n)/b
            var dleft = left.Define();

            checked
            {
                return new Fraction(dleft.numerator * right, dleft.denominator);
            }
        }

        private static Fraction DivideInt(Fraction left, int right)
        {
            // r = (a/b) / n = a/(b*n)
            var dleft = left.Define();

            checked
            {
                return new Fraction(dleft.numerator, dleft.denominator * right);
            }
        }

        private static Fraction DivideIntInverse(int right, Fraction left)
        {
            // r = n / (a/b ) = (n * b) /a
            var dleft = left.Define();

            checked
            {
                return new Fraction(dleft.denominator * right, dleft.numerator);
            }
        }

        public static Fraction operator *(int left, Fraction right)
        {
            return MultiplyInt(right, left);
        }

        public static decimal operator *(Fraction left, decimal right)
        {
            return left.ToDecimal() * right;
        }

        /// <summary>
        /// Performs devision. If the right side is zero an ArithmeticException will be throwned 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator /(Fraction left, Fraction right)
        {
            return Divide(left, right);
        }

        /// <summary>
        /// Performs devision. If the right side is zero an ArithmeticException will be throwned 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator /(Fraction left, int right)
        {
            return DivideInt(left, right);
        }

        /// <summary>
        /// Performs devision. If the right side is zero an ArithmeticException will be throwned 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator /(int left, Fraction right)
        {
            return DivideIntInverse(left, right);
        }

        private static Fraction Divide(Fraction left, Fraction right)
        {
            var dleft = left.Define();
            var dright = right.Define();

            if (dright.IsZero())
            {
                throw new ArithmeticException("Cannot divide by zero");
            }

            checked
            {
                var numerator = dleft.numerator * dright.denominator; // crossed multiplication
                var denominator = dleft.denominator * dright.numerator; // crossed multiplication

                return new Fraction(numerator, denominator);
            }
        }

        private static Fraction Multiply(Fraction left, Fraction right)
        {
            var dleft = left.Define();
            var dright = right.Define();

            checked
            {
                var numerator = dleft.numerator * dright.numerator;
                var denominator = dleft.denominator * dright.denominator;

                return new Fraction(numerator, denominator);
            }
        }

        public static Fraction operator ++(Fraction value)
        {
            return value.Increment();
        }

        public static Fraction operator --(Fraction value)
        {
            return value.Decrement();
        }

        public Fraction Increment()
        {
            // (a / b) + 1 = (a+b) / b
            return new Fraction(this.numerator + this.denominator, this.denominator);
        }

        public Fraction Increment(int n)
        {
            // (a / b) + n = (a+b*n) / b
            return new Fraction(this.numerator + (this.denominator * new BigInteger(n)), this.denominator);
        }

        public Fraction Decrement()
        {
            // a / b - 1 = (a-b) / b
            return new Fraction(this.numerator - this.denominator, this.denominator);
        }

        public Fraction Decrement(int n)
        {
            // a / b - n = (a-b*n) / b
            return new Fraction(this.numerator - (this.denominator * new BigInteger(n)), this.denominator);
        }

        public static Fraction operator +(Fraction left, Fraction right)
        {
            return Plus(left, right);
        }

        public static Fraction operator +(Fraction left, int right)
        {
            return left.Increment(right);
        }

        public static Fraction operator +(int left, Fraction right)
        {
            return right.Increment(left);
        }

        public static Fraction operator -(Fraction left, Fraction right)
        {
            return Plus(left, right.Negate());
        }

        public static Fraction operator -(Fraction right, int left)
        {
            return right.Decrement(left);
        }

        public static Fraction operator -(int right, Fraction left)
        {
            return new Fraction(left.denominator * new BigInteger(right) - left.numerator, left.denominator);
        }

        private static Fraction Plus(Fraction left, Fraction right)
        {
            var dleft = left.Define();
            var dright = right.Define();

            if (dleft.IsZero())
            {
                return right;
            }
            else if (right.IsZero())
            {
                return left;
            }

            var lcm = LCM(dleft.denominator, dright.denominator); // cannot return less than 1
            var leftFactor = lcm / dleft.denominator;
            var righFactor = lcm / dright.denominator;

            checked
            {
                var numerator = dleft.numerator * leftFactor + dright.numerator * righFactor;

                return new Fraction(numerator, lcm);
            }

        }

        public static bool operator <(Fraction left, Fraction right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Fraction left, Fraction right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Fraction left, Fraction right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Fraction left, Fraction right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(Fraction left, long right)
        {
            return left.CompareTo(new Fraction(right, 1)) < 0;
        }

        public static bool operator >(Fraction left, long right)
        {
            return left.CompareTo(new Fraction(right, 1)) > 0;
        }

        public static bool operator <=(Fraction left, long right)
        {
            return left.CompareTo(new Fraction(right, 1)) <= 0;
        }

        public static bool operator >=(Fraction left, long right)
        {
            return left.CompareTo(new Fraction(right, 1)) >= 0;
        }

        #endregion

        public bool IsUndefined()
        {
            return denominator.IsZero;
        }

        /// <summary>
        /// ensures the fraction is well defined, i.e. no zero denominators, by returning Fraction.Zero
        /// if the fraction has a zero denominator. Meaning that 0/0 => 0
        /// </summary>
        /// <returns></returns>
        private Fraction Define()
        {
            return this.IsZero() ? Fraction.Zero : this;
        }

        public int CompareTo(Fraction other)
        {
            var dthis = this.Define();
            var dother = other.Define();

            checked
            {
                var leftScale = dthis.numerator * dother.denominator;
                var rightScale = dthis.denominator * dother.numerator;

                if (leftScale < rightScale)
                    return -1;
                else if (leftScale > rightScale)
                    return 1;
                else
                    return 0;
            }

        }

        public decimal ToDecimal()
        {
            if (this.denominator.IsOne)
            {
                return (decimal)this.numerator;
            }
            else if (this.IsZero())
            {
                return 0M;
            }
            else
            {
                var nn = (decimal)this.numerator;
                var dn = (decimal)this.denominator;

                return nn / dn;
            }
        }

        public double ToDouble()
        {
            if (this.denominator.IsOne)
            {
                return (double)this.numerator;
            }
            else if (this.IsZero())
            {
                return 0d;
            }
            else
            {
                return (double)this.numerator / (double)this.denominator;
            }
        }

        public double ToDouble(int decimals)
        {
            return Convert.ToDouble(ToDecimal().Round(decimals, RoundingMode.RoundHalfUp));
        }

        /// <summary>
        /// Does not compare value, only representation. 4/2 and 2/1 are diferent fractions.
        /// However becasue constructions reduces the fraction, equals is compatible with value comparation.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = (Fraction)obj;
            return this.numerator == other.numerator && this.denominator == other.denominator;
        }

        public override int GetHashCode()
        {
            return numerator.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", numerator, denominator);
        }

        public Fraction Pow(int exponent)
        {
            if (exponent == 0)
            {
                if (this.IsZero())
                {
                    throw new ArgumentException("Cannot calculate 0^0");
                }
                return Fraction.One;
            }
            else if (this.IsZero() || this.IsOne() || exponent == 1)
            {
                return this;
            }

            if (exponent < 0)
            {
                return this.Invert().Pow(-exponent);
            }
            else
            {
                return new Fraction(BigInteger.Pow(this.numerator, exponent), BigInteger.Pow(this.denominator, exponent));
            }
        }

        public BigInteger Ceilling
        {
            get
            {
                return (numerator + denominator - 1) / denominator;
            }

        }

        public BigInteger Floor
        {
            get
            {
                return numerator / denominator;
            }
        }


    }

}
