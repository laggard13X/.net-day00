using System;

namespace ex00
{
    class Program
    {
        static double Days(double sum, double rate, int year, int month)
        {
            int daysinYear = 365;
            if (DateTime.IsLeapYear(year))
                daysinYear++;
            return ((sum * rate * DateTime.DaysInMonth(year, month)) / (daysinYear * 100));
        }

        static double AnnPay(double sum, double i, int n)
        {
            return ((sum * i * Math.Pow((1 + i), n)) / (Math.Pow((1 + i), n) - 1));
        }

        static double val(int k, int selectedMonth, int month, int year, double sum, double rate, double ap)
        {
            double proc;
            double od;
            double over = 0;

            while (k < selectedMonth)
            {
                if (month > 12)
                {
                    year += month / 12;
                    month = month % 12;
                }
                proc = Days(sum, rate, year, month);
                od = ap - proc;
                sum = sum - od;
                month++;
                k++;
                over += proc;
            }
            return (over);
        }

        static void CreditCalculator(double sum, double rate, int term, int selectedMonth, double payment)
        {
            double i = rate / 12 / 100;
            int year = 2021;
            int month = 5;
            double ap;
            double over;
            ap = AnnPay(sum, i, term);
            over = val(0, selectedMonth, month, year, sum, rate, ap);

            month += (selectedMonth);
            if (month > 12)
            {
                year += month / 12;
                month = month % 12;
            }

            // 1 variant
            double newSum = sum - (ap * selectedMonth) + over - payment;
            double ap_1 = AnnPay(newSum, i, term - selectedMonth);
            double ost_1 = val(selectedMonth, term, month, year, newSum, rate, ap_1) + over;
            Console.WriteLine($"Переплата при уменьшении платежа: {ost_1:0 000.00}р.");

            // 2 variant
            int newTerm =  (int) Math.Log((ap / (ap - i * newSum)), i + 1);
            double ost_2 = val(selectedMonth, selectedMonth + newTerm, month, year, newSum, rate, ap) + over;
            Console.WriteLine($"Переплата при уменьшении срока: {ost_2:0 000.00}р.");

            if (ost_1 == ost_2)
                Console.WriteLine("Переплата одинакова в обоих вариантах.");
            else if (ost_1 < ost_2)
                Console.WriteLine($"Уменьшение платежа выгоднее уменьшения срока на {ost_2 - ost_1:00 000.00}р.");
            else
                Console.WriteLine($"Уменьшение срока выгоднее уменьшения платежа на {ost_1 - ost_2:0 000.00}р.");
        }

        static void Main(string[] args)
        {
                if (args.Length == 5 && double.TryParse(args[0], out double sum) && double.TryParse(args[1], out double rate)
                        && int.TryParse(args[2], out int term) && int.TryParse(args[3], out int selectedMonth)
                        && double.TryParse(args[4], out double payment) && sum > 0 && rate > 0 && term > 0 && selectedMonth > 0
                        && payment > 0 && selectedMonth <= term)
                    CreditCalculator(sum, rate, term, selectedMonth, payment);
                else
                    Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
        }
    }
}
