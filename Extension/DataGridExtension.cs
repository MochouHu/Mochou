using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mochou.Core;
using Newtonsoft.Json;
using static Mochou.Forms.DataGridUtils;

namespace Mochou.Forms.Extension
{
    public static class DataGridExtension
    {
        public static DataGridViewCell getCell(this DataGridView grid, int rowIndex, int columnIndex)
        {
            return grid.Rows[rowIndex].Cells[columnIndex];
        }
        public static object GetValue(this DataGridView grid, int rowIndex, int columnIndex)
        {
            return grid.Rows[rowIndex].Cells[columnIndex].Value;
        }
        public static object GetValue(this DataGridView grid, int rowIndex, DataGridViewColumn column)
        {
            return grid.Rows[rowIndex].Cells[column.Index].Value;
        }
        public static void SetValue(this DataGridView grid, int rowIndex, int columnIndex, object obj)
        {
            grid.Rows[rowIndex].Cells[columnIndex].Value = obj;
        }
        public static void SetValue(this DataGridView grid, int rowIndex, DataGridViewColumn column, object obj)
        {
            grid.Rows[rowIndex].Cells[column.Index].Value = obj;
        }
        public static void SetValue(this DataGridView grid, int rowIndex, Dictionary<DataGridViewColumn, object> dic)
        {
            for (var i = 0; i < grid.ColumnCount; i++)
            {
                if (dic.ContainsKey(grid.Columns[i]))
                {
                    grid.SetValue(rowIndex, i, dic[grid.Columns[i]]);
                }
            }
        }
        public static void SetValue(this DataGridView grid, int rowIndex, IDictionary<string, object> dic)
        {
            dic = T.ToLowerKeyDictionary(dic);
            for (var i = 0; i < grid.ColumnCount; i++)
            {
                string key = grid.Columns[i].Name.ToLower();
                if (dic.ContainsKey(key))
                {
                    grid.SetValue(rowIndex, i, dic[key]);
                }
                else if (key.StartsWith("col"))
                {
                    key = key.Substring(3);
                    if (dic.ContainsKey(key))
                    {
                        grid.SetValue(rowIndex, i, dic[key]);
                        continue;
                    }
                }
                else { 
                    key = grid.Columns[i].HeaderText.ToLower();
                    if (dic.ContainsKey(key))
                    {
                        grid.SetValue(rowIndex, i, dic[key]);
                    }
                }
            }
        }
        public static void SetValue(this DataGridView grid, int rowIndex, object dic)
        {
            SetValue(grid, rowIndex, T.ToDictionary(dic));
        }

        public static int AddRow(this DataGridView grid, object obj, Object tag = null)
        {
            IDictionary<String, Object> dic = JsonConvert.DeserializeObject<IDictionary<String, Object>>(JsonConvert.SerializeObject(obj));
            return AddRow(grid, T.ToDictionary(obj), tag);
        }
        public static int AddRow(this DataGridView grid, IDictionary<string, object> dic, object tag = null)
        {
            var index = grid.Rows.Add();
            grid.Rows[index].Tag = tag;
            grid.SetValue(index, dic);
            grid.FirstDisplayedScrollingRowIndex = grid.Rows.Count - 1;
            grid.Rows[grid.Rows.Count - 1].Selected = true;
            grid.CurrentCell = grid[0, grid.Rows.Count - 1];
            return index;
        }

        public static int AddRow(this DataGridView grid, Dictionary<DataGridViewColumn, object> dic, object tag = null)
        {
            var index = grid.Rows.Add();
            grid.Rows[index].Tag = tag;
            for (var i = 0; i < grid.ColumnCount; i++)
            {
                if (dic.ContainsKey(grid.Columns[i]))
                {
                    grid.SetValue(index, i, dic[grid.Columns[i]]);
                }
            }
            return index;
        }

        public static List<O> ToList<O>(this DataGridView dataGridView, GridRowSelected selected = default(GridRowSelected)) where O : new()
        {
            return DataGridUtils.ToList<O>(dataGridView, selected);
        }
    }
}
