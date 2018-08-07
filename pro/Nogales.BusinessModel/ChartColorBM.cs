using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    /// <summary>
    /// Colors used in the chart
    /// </summary>
    public static class ChartColorBM
    {
        /// <summary>
        /// List of colors
        /// </summary>
        public static string[] Colors
        {
            get
            {
                return new string[]
                {
                    //Total main Grocery
                    //"#ad2bc5", //00 
                    "#9ACD32", //00
                    //"#2196f3", //01
                    "#ffc04d", //01
                    //Total main Produce 
                    //"#f4c30a", //02
                    "#228B22", //02
                    //"#69b3d6", //03
                    "#ee7600", //03
                    //Total Drilldown 
                     "#1f77b4", //04 
                     "#aec7e8", //05 
                    //Category main Grocery
                    "#c74b4c", //06
                    "#84b364", //07
                    //Category main Produce
                    "#41c5db", //08
                    "#6faee7", //09
                    //Category drilldown 
                    "#02789F", //10
                    "#00A99C", //11

                    //Sales Person main
                    "#dc3912", //12
                    "#ff9900", //13
                    //Sales Person drill down TOP 5
                    "#006eb9", //14
                    "#f26c52", //15
                    //Sales Person drill down Bottom 5
                    "#006eb9", //16
                    "#f26cf2", //17
                    //Total main Grocery
                    "#ad2bc5", //00 
                    "#2196f3", //01
                    //Total main Produce 
                    "#f4c30a", //02
                    "#69b3d6", //03
                    //Total Drilldown 
                     "#1f77b4", //04 
                     "#aec7e8", //05 
                    //Category main Grocery
                    "#c74b4c", //06
                    "#84b364", //07
                    //Category main Produce
                    "#41c5db", //08
                    "#6faee7", //09
                    //Category drilldown 
                    "#02789F", //10
                    "#00A99C", //11

                    //Sales Person main
                    "#dc3912", //12
                    "#ff9900", //13
                    //Sales Person drill down TOP 5
                    "#006eb9", //14
                    "#f26c52", //15
                    //Sales Person drill down Bottom 5
                    "#006eb9", //16
                    "#f26cf2", //17
            };
            }


        }

        public static string GrocerryCurrent
        {
            get
            {
                return "#ee7600";
            }
        }
        public static string GrocerryPrevious
        {
            get
            {
                return "#ffc04d";
            }
        }

        public static string ProduceCurrent
        {
            get
            {
                return "#228B22";
            }
        }
        public static string ProducePrevious
        {
            get
            {
                return "#9ACD32";
            }
        }
        public static string CategoryCurrent
        {
            get
            {
                return "#c74b4c";
            }
        }
        public static string CategoryPrevious
        {
            get
            {
                return "#6faee7";
            }
        }

        public static string SalesManCurrent
        {
            get
            {
                return "#FA6500";
            }
        }
        public static string SalesManPrevious
        {
            get
            {
                return "#F7CE04";
            }
        }

        public static string ExpenseCurrent
        {
            get
            {
                return "#FA6500";
            }
        }

        public static string ExpensePrevious
        {
            get
            {
                return "#F7CE04";
            }
        }

        public static string MarginCurrent
        {
            get
            {
                return "#65B4D5";
            }
        }

        public static string MarginPrevious
        {
            get
            {
                return "#F8D002";
            }
        }
    }

    public class ColorsDefinition
    {
        public string Primary { get; set; }
        public string Secondary { get; set; }
        public string Tertiary { get; set; }
    }

    public class ColorsGroceryProduce
    {
        public string PrimaryGrocery { get; set; }
        public string PrimaryProduce { get; set; }

        public string SecondaryGrocery { get; set; }
        public string SecondaryProduce { get; set; }

        public string TertiaryGrocery { get; set; }
        public string TertiaryProduce { get; set; }
    }




    public static class BarColumnChartDistinctColors
    {
        public static List<ColorsGroceryProduce> Colors
        {
            get
            {
           

                return new List<ColorsGroceryProduce>
                {
                    new ColorsGroceryProduce { PrimaryGrocery = "#00a2ed",PrimaryProduce="#0ab0fd", SecondaryGrocery = "#72d2ff",SecondaryProduce="#95ddff"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#f0324b",PrimaryProduce="#ff445d", SecondaryGrocery = "#ff7587",SecondaryProduce="#ffbac3"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#e2c206",PrimaryProduce="#f7d301", SecondaryGrocery = "#fce873",SecondaryProduce="#fff1a2"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#00c4c1",PrimaryProduce="#00eae7", SecondaryGrocery = "#5dfdfb",SecondaryProduce="#b3fcfb"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#906f50",PrimaryProduce="#a48669", SecondaryGrocery = "#cdad8d",SecondaryProduce="#e3c6a8"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#f77306",PrimaryProduce="#ff9136", SecondaryGrocery = "#ffb97f",SecondaryProduce="#ffc799"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#ee78e6",PrimaryProduce="#fb8af3", SecondaryGrocery = "#f3a4ed",SecondaryProduce="#ffdffd"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#79cc00",PrimaryProduce="#88de0b", SecondaryGrocery = "#a8f438",SecondaryProduce="#ccfb87"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#9a407e",PrimaryProduce="#d36db3", SecondaryGrocery = "#fc99dd",SecondaryProduce="#ffc4ed"},
                    new ColorsGroceryProduce { PrimaryGrocery = "#17cde5",PrimaryProduce="#7deefd", SecondaryGrocery = "#b7f7ff",SecondaryProduce="#d8fbff"},
                };
            }
        }
    }


    public static class SalesPersonColors
    {
        public static List<ColorsDefinition> Colors
        {

            get
            {
                return new List<ColorsDefinition>
                {
                    new ColorsDefinition { Primary = "#9acd32", Secondary = "#bcde78", Tertiary = "#deefbb" },
                    new ColorsDefinition { Primary = "#1f77b4", Secondary = "#6ba5ce", Tertiary = "#b5d2e6" },
                    new ColorsDefinition { Primary = "#ee7600", Secondary = "#f4a557", Tertiary = "#f9d2ab" },
                    new ColorsDefinition { Primary = "#ffc04d", Secondary = "#ffd58a", Tertiary = "#ffeac4" },
                    new ColorsDefinition { Primary = "#228b22", Secondary = "#6db36d", Tertiary = "#b6d9b6" },
                    new ColorsDefinition { Primary = "#f97a7a", Secondary = "#fba7a7", Tertiary = "#fdd3d3" },
                    new ColorsDefinition { Primary = "#7fcb7f", Secondary = "#abddab", Tertiary = "#d5eed5" },
                    new ColorsDefinition { Primary = "#8080ff", Secondary = "#ababff", Tertiary = "#d5d5ff" },
                    new ColorsDefinition { Primary = "#e9e94d", Secondary = "#f1f18a", Tertiary = "#f8f8c4" },
                    new ColorsDefinition { Primary = "#ff80ff", Secondary = "#ffabff", Tertiary = "#ffd5ff" },
                     };
            }
        }

    }

    public static class TopBottom25GraphColors
    {
        public static List<ColorsDefinition> Colors
        {

            get
            {
                return new List<ColorsDefinition>
                {
                    new ColorsDefinition { Primary = "#66BC29", Secondary = "#bcde78", Tertiary = "#deefbb" },
                    new ColorsDefinition { Primary = "#E81E75", Secondary = "#FF61C3", Tertiary = "#b5d2e6" },
                    new ColorsDefinition { Primary = "#6ba5ce", Secondary = "#7FB3D5", Tertiary = "#f9d2ab" },
                    new ColorsDefinition { Primary = "#6db36d", Secondary = "#C9E39C", Tertiary = "#ffeac4" },
                    new ColorsDefinition { Primary = "#f4a557", Secondary = "#ffd58a", Tertiary = "#b6d9b6" },
                    new ColorsDefinition { Primary = "#00ADDC", Secondary = "#86CDFE", Tertiary = "#fdd3d3" },
                    new ColorsDefinition { Primary = "#1AB39F", Secondary = "#73D1B7", Tertiary = "#d5eed5" },
                    new ColorsDefinition { Primary = "#F8766D", Secondary = "#fba7a7", Tertiary = "#d5d5ff" },
                    new ColorsDefinition { Primary = "#6db36d", Secondary = "#abddab", Tertiary = "#f8f8c4" },
                    new ColorsDefinition { Primary = "#738AC8", Secondary = "#ababff", Tertiary = "#ffd5ff" },
                    new ColorsDefinition { Primary = "#E74C3C", Secondary = "#fba7a7", Tertiary = "#deefbb" },
                    new ColorsDefinition { Primary = "#bcde78", Secondary = "#f1f18a", Tertiary = "#b5d2e6" },
                    new ColorsDefinition { Primary = "#C39BD3", Secondary = "#ffabff", Tertiary = "#f9d2ab" },
                    new ColorsDefinition { Primary = "#6ba5ce", Secondary = "#C4C7C8", Tertiary = "#ffeac4" },
                    new ColorsDefinition { Primary = "#f4a557", Secondary = "#FFC775", Tertiary = "#b6d9b6" },
                    new ColorsDefinition { Primary = "#FEB80A", Secondary = "#ffd58a", Tertiary = "#fdd3d3" },
                    new ColorsDefinition { Primary = "#D5C296", Secondary = "#9D986D", Tertiary = "#d5eed5" },
                    new ColorsDefinition { Primary = "#EB99A9", Secondary = "#fba7a7", Tertiary = "#d5d5ff" },
                    new ColorsDefinition { Primary = "#CED54B", Secondary = "#f1f18a", Tertiary = "#f8f8c4" },
                    new ColorsDefinition { Primary = "#76BAB2", Secondary = "#A0B9AF", Tertiary = "#ffd5ff" },
                    new ColorsDefinition { Primary = "#A0B9AF", Secondary = "#fba7a7", Tertiary = "#fdd3d3" },
                    new ColorsDefinition { Primary = "#C5ACBE", Secondary = "#ffabff", Tertiary = "#d5eed5" },
                    new ColorsDefinition { Primary = "#A74DC3", Secondary = "#ffabff", Tertiary = "#d5d5ff" },
                    new ColorsDefinition { Primary = "#FFC70E", Secondary = "#f1f18a", Tertiary = "#f8f8c4" },
                    new ColorsDefinition { Primary = "#9D986D", Secondary = "#ffabff", Tertiary = "#ffd5ff" },
                };

                //return new List<ColorsDefinition>
                //{
                //    new ColorsDefinition { Primary = "#fa4451", Secondary = "#bcde78", Tertiary = "#deefbb" },
                //    new ColorsDefinition { Primary = "#dc0469", Secondary = "#6ba5ce", Tertiary = "#b5d2e6" },
                //    new ColorsDefinition { Primary = "#ee7600", Secondary = "#f4a557", Tertiary = "#f9d2ab" },
                //    new ColorsDefinition { Primary = "#c230af", Secondary = "#ffd58a", Tertiary = "#ffeac4" },
                //    new ColorsDefinition { Primary = "#6a009e", Secondary = "#6db36d", Tertiary = "#b6d9b6" },
                //    new ColorsDefinition { Primary = "#ed113f", Secondary = "#fba7a7", Tertiary = "#fdd3d3" },
                //    new ColorsDefinition { Primary = "#c8a7d6", Secondary = "#abddab", Tertiary = "#d5eed5" },
                //    new ColorsDefinition { Primary = "#6eafd7", Secondary = "#ababff", Tertiary = "#d5d5ff" },
                //    new ColorsDefinition { Primary = "#0199a7", Secondary = "#f1f18a", Tertiary = "#f8f8c4" },
                //    new ColorsDefinition { Primary = "#6e932c", Secondary = "#ffabff", Tertiary = "#ffd5ff" },
                //    new ColorsDefinition { Primary = "#0159ba", Secondary = "#bcde78", Tertiary = "#deefbb" },
                //    new ColorsDefinition { Primary = "#049d97", Secondary = "#6ba5ce", Tertiary = "#b5d2e6" },
                //    new ColorsDefinition { Primary = "#ae1436", Secondary = "#f4a557", Tertiary = "#f9d2ab" },
                //    new ColorsDefinition { Primary = "#f781be", Secondary = "#ffd58a", Tertiary = "#ffeac4" },
                //    new ColorsDefinition { Primary = "#ac70b8", Secondary = "#6db36d", Tertiary = "#b6d9b6" },
                //    new ColorsDefinition { Primary = "#006079", Secondary = "#fba7a7", Tertiary = "#fdd3d3" },
                //    new ColorsDefinition { Primary = "#3dcac2", Secondary = "#abddab", Tertiary = "#d5eed5" },
                //    new ColorsDefinition { Primary = "#338d43", Secondary = "#ababff", Tertiary = "#d5d5ff" },
                //    new ColorsDefinition { Primary = "#009deb", Secondary = "#f1f18a", Tertiary = "#f8f8c4" },
                //    new ColorsDefinition { Primary = "#00473b", Secondary = "#ffabff", Tertiary = "#ffd5ff" },
                //    new ColorsDefinition { Primary = "#3c0188", Secondary = "#fba7a7", Tertiary = "#fdd3d3" },
                //    new ColorsDefinition { Primary = "#01826d", Secondary = "#abddab", Tertiary = "#d5eed5" },
                //    new ColorsDefinition { Primary = "#851a99", Secondary = "#ababff", Tertiary = "#d5d5ff" },
                //    new ColorsDefinition { Primary = "#76243b", Secondary = "#f1f18a", Tertiary = "#f8f8c4" },
                //    new ColorsDefinition { Primary = "#f68121", Secondary = "#ffabff", Tertiary = "#ffd5ff" },
                //};
            }
        }

    }
}
