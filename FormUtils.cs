using Mochou.Core;
using Mochou.Forms.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Mochou.Forms
{
    public class FormUtils
    {
        //public static readonly Type[] FORM_TYPES = {
        //    typeof(TextBox),typeof(ComboBox),typeof(RadioButton),
        //    typeof(CheckBox),typeof(CheckedListBox),typeof(DataGridView),
        //    typeof(DateTimePicker),typeof(DomainUpDown),typeof(ListBox),
        //    typeof(ListView),typeof(NumericUpDown),typeof(MonthCalendar),
        //    typeof(MaskedTextBox),typeof(CheckBoxGroup)
        //};

        /// <summary>
        /// 将control当做一个表单，转换为一个对象
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static O FormToObject<O>(Control control) where O : new() {
            Dictionary<String, Object> dic = FormToDictionary(control);
            return JObject.FromObject(dic).ToObject<O>();
        }
        public static Dictionary<String, Object> FormToDictionary(Control control) 
        {
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            foreach (Control child in control.Controls)
            {
                if (child is TextBox || child is ComboBox || child is DomainUpDown || child is MaskedTextBox)
                {
                    dic[child.Name] = child.Text;
                }
                else if (child is RadioButton)
                {
                    dic[child.Name] = (child as RadioButton).Checked;
                }
                else if (child is CheckBox)
                {
                    dic[child.Name] = (child as CheckBox).Checked;
                }
                else if (child is CheckBoxGroup)
                {
                    dic[child.Name] = GetCheckBoxGroupValues(child);
                }
                else if (child is ListBox)
                {
                    List<String> ls = new List<string>();
                    foreach (var item in ((ListBox)child).SelectedItems)
                    {
                        ls.Add(T.ToString(item));
                    }
                    dic[child.Name] = ls;
                }
                else if (child is DateTimePicker)
                {
                    DateTimePicker picker = (DateTimePicker)child;
                    dic[child.Name] = picker.Value;
                }
                else if (child is NumericUpDown)
                {
                    dic[child.Name] = ((NumericUpDown)child).Value;
                }
                else if (child is ListView)
                {
                    List<String> ls = new List<string>();
                    foreach (var item in ((ListView)child).SelectedItems)
                    {
                        ls.Add(T.ToString(item));
                    }
                    dic[child.Name] = ls;
                }
                else if (child is MonthCalendar)
                {
                    //后续完善怎么处理
                    MonthCalendar calendar = (MonthCalendar)child;
                    dic[child.Name] = calendar.Text;
                }
                else if (child is DataGridView)
                {
                    DataGridView dataGridView = (DataGridView)child;
                    dic.Add(child.Name, DataGridUtils.ToList<Dictionary<String, Object>>(dataGridView));
                }
                else {
                    Dictionary<String, Object> dic2 = FormToDictionary(child);
                    foreach (var item in dic2)
                    {
                        if (!dic.ContainsKey(item.Key))
                            dic.Add(item.Key, item.Value);
                    }
                }
            }

            //if (dic is O) {
            //    return dic as O;
            //}
            return dic;
        }
        
        public static dynamic FormToObject(Control control)
        {
            return Utils.ToDynamic(FormToDictionary(control));
        }

        /// <summary>
        /// 获取控件中CheckBox的值组成的数组
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static List<String> GetCheckBoxGroupValues(Control control) {
            List<String> vals = new List<string>();
            foreach (Control child in control.Controls) 
                if (child is CheckBox)
                    vals.Add(child.Name);
                else 
                    vals.AddRange(GetCheckBoxGroupValues(child));
            return vals;
        }

        public static void ObjectToForm(Control control, Object vals) {
            JObject jObject = JObject.FromObject(vals);
            foreach (String key in ((IDictionary<String, JToken>)jObject).Keys) { 

            }
        }
    }


    public class FU : FormUtils { }
}
