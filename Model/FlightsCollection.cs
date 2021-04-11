using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AirportSimulation.Model
{
    public class FlightsCollection : IEnumerable<Flight>
    {
        private readonly List<Flight> _Flights;
        private readonly PlaneService _PlaneService;

        public FlightsCollection(PlaneService service)
        {
            _Flights = new List<Flight>();
            _PlaneService = service;
        }

        public List<FlightCsvRecord> GenerateSchedule(int number, int days)
        {
            var cities = new[] { "Пермь", "Москва", "Санкт-Петербург", "Омск", "Екатеринбург" };
            var result = new List<FlightCsvRecord>();
            var random = new Random();

            for (int i = 0; number > i; i++)
            {
                var flight = new FlightCsvRecord
                {
                    PlaneType = (Plane.Type)random.Next(1, Plane.TypeAmount),
                    FlightType = (Flight.Type)random.Next(Flight.TypeAmount),
                    Date = DateTime.MinValue + new TimeSpan(random.Next(days), random.Next(24), random.Next(60), random.Next(60)),
                    City = cities[random.Next(cities.Length)]
                };
                result.Add(flight);
            }

            return result;
        }

        public void Save(string fileToSave, IEnumerable<FlightCsvRecord> schedule)
        {
            var culture = CultureInfo.GetCultureInfo("ru");
            var config = new CsvConfiguration(culture) { HasHeaderRecord = false };

            using (var writer = new StreamWriter(fileToSave))
            using (var csv = new CsvWriter(writer, config))
                csv.WriteRecords(schedule);
        }

        public void Load(string fileToLoad)
        {
            var culture = CultureInfo.GetCultureInfo("ru");
            var config = new CsvConfiguration(culture) { HasHeaderRecord = false };
            
            using (var reader = new StreamReader(fileToLoad))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<FlightCsvRecord>();
                foreach (var record in records)
                {
                    var plane = _PlaneService.GetPlaneByType(record.PlaneType);
                    var flight = new Flight
                    {
                        Plane = plane,
                        FlightType = record.FlightType,
                        Date = record.Date,
                        City = record.City
                    };
                    _Flights.Add(flight);
                }
            }
        }

        public bool TryLoad(string fileToLoad, out LoadResult result)
        {
            try
            {
                Load(fileToLoad);
                result = LoadResult.Success();
                return true;
            }
            catch(TypeConverterException e)
            {
                var resultMessage = $"Строка {e.Context.Parser.Row}, ";
                switch (e.MemberMapData.Index)
                {
                    case FlightCsvRecord.PLANE_TYPE_INDEX:
                        var planes = string.Join(",", Enum.GetNames(typeof(Plane.Type)));
                        resultMessage += $"Неверное имя модели самолета. Можно использовать только {planes}";
                        break;
                    case FlightCsvRecord.FLIGHT_TYPE_INDEX:
                        var flights = string.Join(",", Enum.GetNames(typeof(Flight.Type)));
                        resultMessage += $"Неверное имя типа полета. Можно использовать только {flights}";
                        break;
                }
                result = LoadResult.Error(resultMessage);
            }
            catch (CsvHelper.MissingFieldException e)
            {
                var resultMessage = $"Строка {e.Context.Parser.Row}, ";
                switch (e.Context.Reader.CurrentIndex)
                {
                    case FlightCsvRecord.PLANE_TYPE_INDEX:
                        resultMessage += $"Нет столбца имя модели самолета";
                        break;
                    case FlightCsvRecord.FLIGHT_TYPE_INDEX:
                        resultMessage += $"Нет столбца типа полета";
                        break;
                    case FlightCsvRecord.DATE_INDEX:
                        resultMessage += "Нет столбца даты самолета";
                        break;
                    case FlightCsvRecord.CITY_INDEX:
                        resultMessage += $"Нет столбца города";
                        break;
                }
                result = LoadResult.Error(resultMessage);
            }
            catch (ReaderException e)
            {
                var resultMessage = $"Строка {e.Context.Parser.Row}, ";
                switch (e.Context.Reader.CurrentIndex)
                {
                    case FlightCsvRecord.DATE_INDEX:
                        resultMessage += "Неверный формат даты. Должен быть 01.01.0001 00:00:00";
                        break;
                    case FlightCsvRecord.CITY_INDEX:
                        resultMessage += $"Неверный формат города. {e.Message}";
                        break;
                }
                result = LoadResult.Error(resultMessage);
            }
            catch(Exception e)
            {
                result = LoadResult.Error(e.Message);
            }

            return false;
        }

        public IEnumerator<Flight> GetEnumerator() => _Flights.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _Flights.GetEnumerator();

        public struct LoadResult
        {
            public bool HasError { get; private set; }
            public string ErrorMessage { get; private set; }

            public static LoadResult Error(string errorMessage) => new LoadResult(true, errorMessage);
            public static LoadResult Success() => new LoadResult(false, string.Empty);

            public LoadResult(bool hasError, string errorMessage)
            {
                HasError = hasError;
                ErrorMessage = errorMessage;
            }
        }
    }
}
