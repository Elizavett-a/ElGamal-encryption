using System.Text;
using System.Collections;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TI_Lab3
{
    public class Elgamal
    {
        public static string CheckForNum(string str)
        {
            var result = new StringBuilder();
            foreach (char symbol in str)
            {
                if (char.IsDigit(symbol))
                {
                    result.Append(symbol);
                }
            }
            return result.ToString();
        }

        public static bool CheckForK(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a == 1;
        }

        public static bool CheckForX(int a, int p)
        {
            return a < (p - 1) && a > 1;
        }

        // Проверка на простое число
        public static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0) return false;

            for (int i = 3; i * i < n; i += 2)
            {
                if (n % i == 0) return false;
            }

            return true;
        }

        //Возведение в степень по модулю
        public static int ModPow(int a, int exponent, int modulus)
        {
            if (modulus == 1) return 0;

            int result = 1;
            a = a % modulus;

            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                    result = (result * a) % modulus;

                exponent >>= 1;
                a = (a * a) % modulus;
            }
            return result;
        }

        public static int ModInverse(int num, int modulus)
        {
            if (num == 0)
                throw new ArgumentException("Ошибка при вычислении!");

            int originalModulus = modulus, temp, quotient;
            int prevCoeff = 0, currentCoeff = 1;

            if (modulus == 1)
                return 0;

            while (num > 1)
            {
                quotient = num / modulus;
                temp = modulus;
                modulus = num % modulus;
                num = temp;
                temp = prevCoeff;
                prevCoeff = currentCoeff - quotient * prevCoeff;
                currentCoeff = temp;
            }

            if (currentCoeff < 0)
                currentCoeff += originalModulus;

            if (num != 1)
                throw new ArgumentException($"Обратный элемент для {num} mod {modulus} не существует");

            return currentCoeff;
        }

        public static int ComputeModularExpression(int b, int a, int x, int p)
        {
            int aPowX = ModPow(a, Math.Abs(x), p);
            int invAPowX = ModInverse(aPowX, p);

            return (b * invAPowX) % p;
        }

        //Поиск первообразных
        public static List<int> FindPrimitiveRoots(int p)
        {
            if (!Elgamal.IsPrime(p))
                return new List<int>();

            int phi = p - 1;
            var factors = PrimeFactors(phi).Distinct().ToList();
            var primitiveRoots = new List<int>();

            for (int g = 2; g < p; g++)
            {
                bool isPrimitiveRoot = true;
                foreach (var f in factors)
                {
                    if (ModPow(g, phi / f, p) == 1)
                    {
                        isPrimitiveRoot = false;
                        break;
                    }
                }

                if (isPrimitiveRoot)
                    primitiveRoots.Add(g);
            }

            return primitiveRoots;
        }

        //Проверка числа на то, что это первообразное
        public static bool IsPrimitiveRoot(int g, int p)
        {
            if (!IsPrime(p))
                throw new ArgumentException("p должно быть простым числом");

            if (g <= 0 || g >= p)
                return false;

            int phi = p - 1;
            var factors = PrimeFactors(phi);
            return IsPrimitiveRoot(g, p, phi, factors);
        }

        private static bool IsPrimitiveRoot(int g, int p, int phi, List<int> primeFactors)
        {
            foreach (int q in primeFactors)
            {
                if (ModPow(g, phi / q, p) == 1)
                    return false;
            }
            return true;
        }

        // Разложение числа на простые множители
        public static List<int> PrimeFactors(int n)
        {
            List<int> factors = new List<int>();

            while (n % 2 == 0)
            {
                factors.Add(2);
                n /= 2;
            }

            int i = 3;
            double maxFactor = Math.Sqrt(n) + 1;
            while (i <= maxFactor)
            {
                while (n % i == 0)
                {
                    factors.Add(i);
                    n /= i;
                    maxFactor = Math.Sqrt(n) + 1;
                }
                i += 2;
            }

            if (n > 1)
                factors.Add(n);

            return factors;
        }
    }
}