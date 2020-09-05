using Mochou.Core;
using Mochou.Forms.Extension;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mochou.Forms
{
    public class DataGridUtils
    {
        public delegate bool GridRowSelected(DataGridViewRow row);
        public static void LoadData(DataGridView dataGridView, List<IDictionary<string, object>> data, object maping = null)
        {
            IDictionary<string, object> dicMaping = new Dictionary<string, object>();
            if (maping != null)
            {
                dicMaping = T.ToDictionary(maping);
            }
            dataGridView.Rows.Clear();

            if (data.Count == 0)
                return;
            List<IDictionary<string, object>> handData = new List<IDictionary<string, object>>();
            //转换为小写
            foreach (var it in data)
                handData.Add(T.ToLowerKeyDictionary(it));

            Dictionary<string, DataGridViewColumn> columnKeys = new Dictionary<string, DataGridViewColumn>();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                //先验证Maping
                if (dicMaping.ContainsKey(column.Name))
                {
                    columnKeys.Add(T.ToString(dicMaping[column.Name]).ToLower(), column);
                }

                string key = column.Name.ToLower();
                if (handData[0].ContainsKey(key))
                {
                    columnKeys.Add(key, column);
                    continue;
                }
                if (key.StartsWith("col"))
                {
                    key = key.Substring(3);
                    if (handData[0].ContainsKey(key))
                    {
                        columnKeys.Add(key, column);
                        continue;
                    }
                }

                key = column.HeaderText.ToLower();
                if (handData[0].ContainsKey(key))
                {
                    columnKeys.Add(key, column);
                    continue;
                }
            }

            handData.ForEach(it =>
            {
                int index = dataGridView.Rows.Add();
                foreach (var cols in columnKeys)
                    if (it.ContainsKey(cols.Key))
                        dataGridView.SetValue(index, cols.Value, it[cols.Key]);
            });
        }
        public static void LoadData<O>(DataGridView dataGridView, List<O> data, object maping = null)
        {
            IDictionary<string, object> dicMaping = new Dictionary<string, object>();
            if (maping != null) {
                dicMaping = T.ToDictionary(maping);
            }

            List<IDictionary<string, object>> handData = new List<IDictionary<string, object>>();
            //转换为小写
            foreach (var it in data) {
                handData.Add(new Dictionary<string, object>());
                var dic = T.ToDictionary(it);
                foreach(var dicit in dic)
                    handData[handData.Count - 1].Add(dicit.Key.ToLower(), dicit.Value);
            }

            dataGridView.Rows.Clear();

            Dictionary<string, DataGridViewColumn> columnKeys = new Dictionary<string, DataGridViewColumn>();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                //先验证Maping
                if (dicMaping.ContainsKey(column.Name)) {
                    columnKeys.Add(T.ToString(dicMaping[column.Name]).ToLower(), column);
                }

                string key = column.Name.ToLower();
                if (handData[0].ContainsKey(key))
                {
                    columnKeys.Add(key, column);
                    continue;
                }
                if (key.IndexOf("col") == 0)
                {
                    key = key.Substring(3);
                    if (handData[0].ContainsKey(key))
                    {
                        columnKeys.Add(key, column);
                        continue;
                    }
                }
            }

            handData.ForEach(it =>
            {
                int index = dataGridView.Rows.Add();
                foreach (var cols in columnKeys)
                    if (it.ContainsKey(cols.Key))
                        dataGridView.SetValue(index, cols.Value, it[cols.Key]);
            });
        }
        public static List<O> ToList<O>(DataGridView dataGridView, GridRowSelected selected = default(GridRowSelected)) where O : new()
        {
            List<O> jArray = new List<O>();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (selected != default && !selected(row)) continue;
                JObject jObject = new JObject();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    String attr = dataGridView.Columns[cell.ColumnIndex].Name;
                    if (attr.IndexOf("col") >= 0)
                    {
                        attr = attr.Substring(3);
                    }
                    jObject.Add(attr, T.ToString(cell.Value));
                }
                jArray.Add(jObject.ToObject<O>());
            }
            return jArray;
        }

        public static List<O> ToListByHeaderText<O>(DataGridView dataGridView, GridRowSelected selected = default(GridRowSelected)) where O : new()
        {
            List<O> jArray = new List<O>();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (selected != default && !selected(row)) continue;
                JObject jObject = new JObject();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    String attr = dataGridView.Columns[cell.ColumnIndex].HeaderText;
                    jObject.Add(attr, T.ToString(cell.Value));
                }
                jArray.Add(jObject.ToObject<O>());
            }
            return jArray;
        }
    }
}
