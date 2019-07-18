using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Core;
using System.Linq;

namespace Fonts
{
    public class SomeBusinessLogic : CoreBusiness
    {
        public List<FontItemRow> GetFontList(Dictionary<string, char> dict, string fontFamily)
        {
            var temp = new List<FontItemRow>();
            var col = 0;
            var row = 0;

            foreach (var key in dict.Keys)
            {
                if (col == 3)
                {
                    col = 0;
                    row++;
                }

                FontItemRow rowItem = null;
                if (col == 0)
                {
                    rowItem = new FontItemRow() { Row = row };
                    temp.Add(rowItem);
                }
                else
                {
                    rowItem = temp.First(x => x.Row == row);
                }

                var item = new FontItem()
                {
                    FriendlyName = key,
                    FontFamily = fontFamily,
                    Unicode = dict[key].ToString()
                };

                switch (col)
                {
                    case 0:
                        rowItem.Item1 = item;
                        break;
                    case 1:
                        rowItem.Item2 = item;
                        break;
                    case 2:
                        rowItem.Item3 = item;
                        break;
                }

                col++;
            }

            return temp;
        }
    }
}