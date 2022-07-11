
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DigitalRain.Config
{
    using Raindrop;
    using Grid.ColumnNumberPickers;

    public class ConfigurationProfile
    {
        private static Dictionary<string, IColumnNumberPicker> _columnNumberPickers = new Dictionary<string, IColumnNumberPicker>
        {
            { "random", new RandomColumnNumberPicker(columnCount: 50, lowWaterMark: 10) },
            { "roundrobin", new RoundRobinColumnNumberPicker(columnCount: 50) }
        };

        private static Dictionary<string, ColorCalculatorFactory> _colorCalculatorFactories = new Dictionary<string, ColorCalculatorFactory>
        {
            {
                "matrixgreen", new ColorCalculatorFactory(
                    startColor: Color.White,
                    endColor: Color.GreenYellow,
                    lerpTime: 400
                )
            },
            {
                "jamescomopink", new ColorCalculatorFactory(
                    startColor: Color.White,
                    endColor: HexColor("FF1694"),
                    lerpTime: 400
                )
            },
            {
                "red", new ColorCalculatorFactory(
                    startColor: Color.White,
                    endColor: Color.Red,
                    lerpTime: 400
                )
            },
            {
                "xmas", new ColorCalculatorFactory(
                    startColor: Color.Red,
                    endColor: Color.Green,
                    lerpTime: 1000
                )
            }
        };

        private static Dictionary<string, RandomSymbolFactory> _symbolFactories = new Dictionary<string, RandomSymbolFactory>
        {
            {
                "randomalphabet", new RandomSymbolFactory(
                    SymbolPools.EnglishAlphanumericUpperSymbols()
                )
            }
        };

        private static Dictionary<string, RandomLifespanFactory> _lifespanFactories = new Dictionary<string, RandomLifespanFactory>
        {
            {
                "random", new RandomLifespanFactory(lifespanMin: 2800, lifespanMax: 5500)
            }
        };

        public static readonly Dictionary<string, IRaindropFactory> RaindropFactories = new Dictionary<string, IRaindropFactory>
        {
            {
                "randomgreen", new StandardRaindropFactory(
                    symbolFactory: _symbolFactories["randomalphabet"],
                    lifespanFactory: _lifespanFactories["random"],
                    colorCalculatorFactory: _colorCalculatorFactories["matrixgreen"]
                )
            },
            {
                "randompink", new StandardRaindropFactory(
                    symbolFactory: _symbolFactories["randomalphabet"],
                    lifespanFactory: _lifespanFactories["random"],
                    colorCalculatorFactory: _colorCalculatorFactories["jamescomopink"]
                )
            },
            {
                "randomred", new StandardRaindropFactory(
                    symbolFactory: _symbolFactories["randomalphabet"],
                    lifespanFactory: _lifespanFactories["random"],
                    colorCalculatorFactory: _colorCalculatorFactories["red"]
                )
            },
            {
                "randomxmas", new StandardRaindropFactory(
                    symbolFactory: _symbolFactories["randomalphabet"],
                    lifespanFactory: _lifespanFactories["random"],
                    colorCalculatorFactory: _colorCalculatorFactories["xmas"]
                )
            },
            {
                "helloworld", new PerGridCoordinateRaindropFactory()
            }
        };

        public static ConfigurationProfile[] ConfigurationProfiles = 
        {
            // Profile 0
            new ConfigurationProfile(
                _columnNumberPickers["random"],
                RaindropFactories["randomgreen"]    
            ),
            // Profile 1
            new ConfigurationProfile(
                _columnNumberPickers["random"],
                RaindropFactories["helloworld"]
            ),
            // Profile 2
            new ConfigurationProfile(
                _columnNumberPickers["roundrobin"],
                RaindropFactories["helloworld"]
            ),
            // Profile 3
            new ConfigurationProfile(
                _columnNumberPickers["random"],
                RaindropFactories["randompink"]
            ),
            // Profile 4
            new ConfigurationProfile(
                _columnNumberPickers["random"],
                RaindropFactories["randomxmas"]
            )
        };

        public IColumnNumberPicker ColumnNumberPicker { get; private set; }
        public IRaindropFactory RaindropFactory { get; private set; }
        private ConfigurationProfile(IColumnNumberPicker columnNumberPicker, IRaindropFactory raindropFactory)
        {
            ColumnNumberPicker = columnNumberPicker;
            RaindropFactory = raindropFactory;
        }

        private static Color HexColor(string hexCode)
        {
            var number = Convert.ToUInt32(hexCode, 16);
            var r = (int)((number >> 16) & 0xFF);
            var g = (int)((number >> 8) & 0xFF);
            var b = (int)((number) & 0xFF);

            // pink = (255, 192, 203)
            Debug.WriteLine("r, g, b = " + (r, g, b).ToString());
            return new Color(r, g, b);
        }
    }
}
