using System;

namespace avitoRequestService
{

    public static class DateTimeFromAvitoString
    {
        public static DateTime dateTimeFromAvitoString(this string strFromAvito)
        {
            DateTime dateValue;

            if (!String.IsNullOrEmpty(strFromAvito))
            {
                var timeString = strFromAvito.Substring(strFromAvito.IndexOf(';') +1);

                if (strFromAvito.Contains("Сегодня"))
                {
                    if (DateTime.TryParse(timeString, out dateValue))
                        return dateValue;
                    else
                        Console.WriteLine("Unable to parse '{0}'.", timeString);
                }

                if (strFromAvito.Contains("Вчера"))
                {
                    if (DateTime.TryParse(timeString, out dateValue))
                        return dateValue.AddDays(-1);
                    else
                        Console.WriteLine("Unable to parse '{0}'.", timeString);
                }

                if (strFromAvito.Contains("Назад"))
                {
                    // if (DateTime.TryParse(timeString, out dateValue))
                    //     return dateValue.AddDays(-1);
                    // else
                        Console.WriteLine("Надо обработать {0}", strFromAvito);
                }


            }
            return new DateTime();
        }
    }

}