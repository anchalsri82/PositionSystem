using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PositionSystem.Implementations;
using PositionSystem.Interfaces;

namespace PositionSystem.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        private static IContainer Container { get; set; }

        public UnitTests()
        {
            var builder = new ContainerBuilder();

            // Register individual components
            builder.RegisterType<TradingPosition>().As<ITradingPosition>();
            builder.RegisterType<CsvReader>().As<ICsvReader>();
            builder.RegisterType<CsvWriter>().As<ICsvWriter>();
            builder.RegisterType<TradingPositionResult>().As<ITradingPositionResult>();
            builder.RegisterType<CsvPositionProvider<ITradingPosition>>().As<IPositionProvider<ITradingPosition>>();
            builder.RegisterType<TradingPositionCalculator<ITradingPosition, TradingPositionAction, ITradingPositionResult>>().As<IReceiver<ITradingPosition, TradingPositionAction, ITradingPositionResult>>();
            builder.RegisterType<CsvReader>();
            builder.RegisterType<CsvWriter>();
            //builder.RegisterInstance(new PositionProvider()).As<IPositionProvider>();


            Container = builder.Build();
        }

        [TestMethod]
        // internal IDictionary<string, int> GetColumnIndices(string[] propertyValues)
        public void GetColumnIndicesTest()
        {
            var testData = new[]
            {
                new
                {
                    ColumnNamesInput = new[] {"TRADER", "BROKER", "SYMBOL", "QUANTITY", "PRICE"},
                    ColumnIndicesExpected = new Dictionary<string, int>
                        {{"TRADER", 0}, {"BROKER", 1}, {"SYMBOL", 2}, {"QUANTITY", 3}, {"PRICE", 4}}
                },
                new
                {
                    ColumnNamesInput = new[] {"BROKER", "TRADER", "SYMBOL", "QUANTITY", "PRICE"},
                    ColumnIndicesExpected = new Dictionary<string, int>
                        {{"BROKER", 0}, {"TRADER", 1}, {"SYMBOL", 2}, {"QUANTITY", 3}, {"PRICE", 4}}
                },
                new
                {
                    ColumnNamesInput = new[] {"SYMBOL", "BROKER", "TRADER", "QUANTITY", "PRICE"},
                    ColumnIndicesExpected = new Dictionary<string, int>
                        {{"SYMBOL", 0}, {"BROKER", 1}, {"TRADER", 2}, {"QUANTITY", 3}, {"PRICE", 4}}
                }
            };
            using (var scope = Container.BeginLifetimeScope())
            {
                var csvReader = scope.Resolve<CsvReader>();
                foreach (var testRow in testData)
                {
                    var actualResult = csvReader.GetColumnIndices(testRow.ColumnNamesInput);
                    Assert.AreEqual(actualResult.Count, testRow.ColumnIndicesExpected.Count);
                    foreach (var key in actualResult.Keys)
                    {
                        Assert.AreEqual(actualResult[key], testRow.ColumnIndicesExpected[key]);
                    }
                }
            }
        }

        [TestMethod]
        // internal T GetObject<T>(string[] propertyValues, IDictionary<string, int> columnIndices)
        public void GetObjectTest()
        {
            var testData = new[]
            {
                new
                {
                    PositionRawInput = new[] {"Joe", "ML", "IBM.N", "100", "50"},
                    ColumnIndicesInput = new Dictionary<string, int>
                        {{"TRADER", 0}, {"BROKER", 1}, {"SYMBOL", 2}, {"QUANTITY", 3}, {"PRICE", 4}},
                    PositionExpected = new TradingPosition
                    {
                        TRADER = "Joe",
                        BROKER = "ML",
                        SYMBOL = "IBM.N",
                        QUANTITY = 100D,
                        PRICE = 50D
                    }
                },
                new
                {
                    PositionRawInput = new[] {"ML", "Joe", "IBM.N", "100", "50"},
                    ColumnIndicesInput = new Dictionary<string, int>
                        {{"BROKER", 0}, {"TRADER", 1}, {"SYMBOL", 2}, {"QUANTITY", 3}, {"PRICE", 4}},
                    PositionExpected = new TradingPosition
                    {
                        TRADER = "Joe",
                        BROKER = "ML",
                        SYMBOL = "IBM.N",
                        QUANTITY = 100D,
                        PRICE = 50D
                    }
                },
                new
                {
                    PositionRawInput = new[] {"IBM.N", "ML", "Joe", "100", "50"},
                    ColumnIndicesInput = new Dictionary<string, int>
                        {{"SYMBOL", 0}, {"BROKER", 1}, {"TRADER", 2}, {"QUANTITY", 3}, {"PRICE", 4}},
                    PositionExpected = new TradingPosition
                    {
                        TRADER = "Joe",
                        BROKER = "ML",
                        SYMBOL = "IBM.N",
                        QUANTITY = 100D,
                        PRICE = 50D
                    }
                }
            };
            using (var scope = Container.BeginLifetimeScope())
            {
                var csvReader = scope.Resolve<CsvReader>();
                foreach (var testRow in testData)
                {
                    var result =
                        csvReader.GetObject<TradingPosition>(testRow.PositionRawInput, testRow.ColumnIndicesInput);
                    Assert.IsTrue(result.TRADER == testRow.PositionExpected.TRADER &&
                                  result.PRICE == testRow.PositionExpected.PRICE &&
                                  result.BROKER == testRow.PositionExpected.BROKER &&
                                  result.SYMBOL == testRow.PositionExpected.SYMBOL &&
                                  result.QUANTITY == testRow.PositionExpected.QUANTITY);
                }
            }
        }

        [TestMethod]
        // internal IEnumerable<T> GetObjects<T>(TextReader sr, bool hasHeaders)
        public void GetObjectsTest()
        {
            var testData = new[]
            {
                new
                {
                    CsvContentInput =
                    "TRADER\tBROKER\tSYMBOL\tQUANTITY\tPRICE\nJoe\tML\tIBM.N\t100\t50\nJoe\tDB\tIBM.N\t-50\t50\nJoe\tCS\tIBM.N\t30\t30\nMike\tCS\tAAPL.N\t100\t20\nMike\tBC\tAAPL.N\t200\t20\nDebby\tBC\tNVDA.N\t500\t20",
                    PositionExpected = new List<ITradingPosition>
                    {
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "ML",
                            SYMBOL = "IBM.N",
                            QUANTITY = 100,
                            PRICE = 50
                        },
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "DB",
                            SYMBOL = "IBM.N",
                            QUANTITY = -50,
                            PRICE = 50
                        },
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "CS",
                            SYMBOL = "IBM.N",
                            QUANTITY = 30,
                            PRICE = 30
                        },
                        new TradingPosition
                        {
                            TRADER = "Mike",
                            BROKER = "CS",
                            SYMBOL = "AAPL.N",
                            QUANTITY = 100,
                            PRICE = 20
                        },
                        new TradingPosition
                        {
                            TRADER = "Mike",
                            BROKER = "BC",
                            SYMBOL = "AAPL.N",
                            QUANTITY = 200,
                            PRICE = 20
                        },
                        new TradingPosition
                        {
                            TRADER = "Debby",
                            BROKER = "BC",
                            SYMBOL = "NVDA.N",
                            QUANTITY = 500,
                            PRICE = 20
                        }
                    }
                },
                new
                {
                    CsvContentInput =
                    "BROKER\tTRADER\tSYMBOL\tQUANTITY\tPRICE\nML\tJoe\tIBM.N\t100\t50\nDB\tJoe\tIBM.N\t-50\t50\nCS\tJoe\tIBM.N\t30\t30\nCS\tMike\tAAPL.N\t100\t20\nBC\tMike\tAAPL.N\t200\t20\nBC\tDebby\tNVDA.N\t500\t20",
                    PositionExpected = new List<ITradingPosition>
                    {
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "ML",
                            SYMBOL = "IBM.N",
                            QUANTITY = 100,
                            PRICE = 50
                        },
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "DB",
                            SYMBOL = "IBM.N",
                            QUANTITY = -50,
                            PRICE = 50
                        },
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "CS",
                            SYMBOL = "IBM.N",
                            QUANTITY = 30,
                            PRICE = 30
                        },
                        new TradingPosition
                        {
                            TRADER = "Mike",
                            BROKER = "CS",
                            SYMBOL = "AAPL.N",
                            QUANTITY = 100,
                            PRICE = 20
                        },
                        new TradingPosition
                        {
                            TRADER = "Mike",
                            BROKER = "BC",
                            SYMBOL = "AAPL.N",
                            QUANTITY = 200,
                            PRICE = 20
                        },
                        new TradingPosition
                        {
                            TRADER = "Debby",
                            BROKER = "BC",
                            SYMBOL = "NVDA.N",
                            QUANTITY = 500,
                            PRICE = 20
                        }
                    }
                }
            };
            using (var scope = Container.BeginLifetimeScope())
            {
                var csvReader = scope.Resolve<CsvReader>();

                foreach (var testRow in testData)
                {
                    var actualResult = csvReader
                        .GetObjects<ITradingPosition>(new StringReader(testRow.CsvContentInput), "\t", true).ToList();
                    Assert.AreEqual(actualResult.Count, testRow.PositionExpected.Count);

                    for (int i = 0; i < 1; i++)
                    {
                        Assert.IsTrue(actualResult[i].BROKER == testRow.PositionExpected[i].BROKER &&
                                      actualResult[i].TRADER == testRow.PositionExpected[i].TRADER &&
                                      actualResult[i].SYMBOL == testRow.PositionExpected[i].SYMBOL &&
                                      actualResult[i].QUANTITY == testRow.PositionExpected[i].QUANTITY
                                      && actualResult[i].PRICE == testRow.PositionExpected[i].PRICE);
                    }
                }

            }
        }

        [TestMethod]
        // internal IEnumerable<TResult> GetBoxPositions()
        public void GetBoxPositionsTest()
        {
            var testData = new[]
            {
                new
                {
                    PositionInput = new List<ITradingPosition>
                    {
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "ML",
                            SYMBOL = "IBM.N",
                            QUANTITY = 100,
                            PRICE = 50
                        },
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "DB",
                            SYMBOL = "IBM.N",
                            QUANTITY = -50,
                            PRICE = 50
                        },
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "CS",
                            SYMBOL = "IBM.N",
                            QUANTITY = 30,
                            PRICE = 30
                        },
                        new TradingPosition
                        {
                            TRADER = "Mike",
                            BROKER = "CS",
                            SYMBOL = "AAPL.N",
                            QUANTITY = 100,
                            PRICE = 20
                        },
                        new TradingPosition
                        {
                            TRADER = "Mike",
                            BROKER = "BC",
                            SYMBOL = "AAPL.N",
                            QUANTITY = 200,
                            PRICE = 20
                        },
                        new TradingPosition
                        {
                            TRADER = "Debby",
                            BROKER = "BC",
                            SYMBOL = "NVDA.N",
                            QUANTITY = 500,
                            PRICE = 20
                        }
                    },
                    ResultExpected = new List<ITradingPositionResult>
                    {
                        new TradingPositionResult
                        {
                            TRADER = "Joe",
                            SYMBOL = "IBM.N",
                            QUANTITY= 50
                        }

                    }
                }
            };

            using (var scope = Container.BeginLifetimeScope())
            {
                foreach (var testRow in testData)
                {
                    var tradingPositionCalculator = (TradingPositionCalculator<ITradingPosition, TradingPositionAction, ITradingPositionResult>) scope.Resolve<IReceiver<ITradingPosition, TradingPositionAction, ITradingPositionResult>>();
                    var actualResult = tradingPositionCalculator.GetBoxPositions(testRow.PositionInput);
                    int i = 0;
                    foreach (var result in actualResult)
                    {
                        Assert.IsTrue(result.SYMBOL == testRow.ResultExpected[i].SYMBOL &&
                                      result.TRADER == testRow.ResultExpected[i].TRADER &&
                                      result.QUANTITY == testRow.ResultExpected[i].QUANTITY);
                        i++;
                    }
                }
            }
        }
        [TestMethod]

        // internal IEnumerable<TResult> GetNetPositions()
        public void GetNetPositionsTest()
        {
            var testData = new[]
            {
                new
                {
                    PositionInput = new List<ITradingPosition>
                    {
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "ML",
                            SYMBOL = "IBM.N",
                            QUANTITY = 100,
                            PRICE = 50
                        },
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "DB",
                            SYMBOL = "IBM.N",
                            QUANTITY = -50,
                            PRICE = 50
                        },
                        new TradingPosition
                        {
                            TRADER = "Joe",
                            BROKER = "CS",
                            SYMBOL = "IBM.N",
                            QUANTITY = 30,
                            PRICE = 30
                        },
                        new TradingPosition
                        {
                            TRADER = "Mike",
                            BROKER = "CS",
                            SYMBOL = "AAPL.N",
                            QUANTITY = 100,
                            PRICE = 20
                        },
                        new TradingPosition
                        {
                            TRADER = "Mike",
                            BROKER = "BC",
                            SYMBOL = "AAPL.N",
                            QUANTITY = 200,
                            PRICE = 20
                        },
                        new TradingPosition
                        {
                            TRADER = "Debby",
                            BROKER = "BC",
                            SYMBOL = "NVDA.N",
                            QUANTITY = 500,
                            PRICE = 20
                        }
                    },
                    ResultExpected = new List<ITradingPositionResult>
                    {
                        new TradingPositionResult
                        {
                            TRADER = "Joe",
                            SYMBOL = "IBM.N",
                            QUANTITY= 80
                        },
                        new TradingPositionResult
                        {
                            TRADER = "Mike",
                            SYMBOL = "AAPL.N",
                            QUANTITY= 300
                        },
                        new TradingPositionResult
                        {
                            TRADER = "Debby",
                            SYMBOL = "NVDA.N",
                            QUANTITY= 500
                        }
                    }
                }
            };
            
            using (var scope = Container.BeginLifetimeScope())
            {
                foreach (var testRow in testData)
                {
                    var tradingPositionCalculator =
                        (TradingPositionCalculator<ITradingPosition, TradingPositionAction, ITradingPositionResult>)
                        scope.Resolve<IReceiver<ITradingPosition, TradingPositionAction, ITradingPositionResult>>();
                    var actualResult = tradingPositionCalculator.GetNetPositions(testRow.PositionInput);
                    int i = 0;
                    foreach (var result in actualResult)
                    {
                        Assert.IsTrue(result.SYMBOL == testRow.ResultExpected[i].SYMBOL &&
                                      result.TRADER == testRow.ResultExpected[i].TRADER &&
                                      result.QUANTITY == testRow.ResultExpected[i].QUANTITY);
                        i++;
                    }
                }
            }
        }
    }
}
