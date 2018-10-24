using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace W2ItemHelp
{
    public partial class MainForm : Form
    {
        private STRUCT_ITEMHELP[]   g_pItemHelp = default(STRUCT_ITEMHELP[]);

        private int                 irg_ItemCount = 0;

        public MainForm()
        {
            InitializeComponent();

            Begin();
        }

        private void Begin()
        {
            g_pItemHelp = new STRUCT_ITEMHELP[BASE.MAX_ITEMLIST];

            for (int i = 0; i < g_pItemHelp.Length; i++)
            {
                g_pItemHelp[i] = STRUCT_ITEMHELP.CraftProperties();
            }
        }

        private void BASE_OpenItemHelp(string _fileName)
        {
            string _Path_ItemHelp_File_ = _fileName;

            try
            {
                
                using (var Sr = new StreamReader(_Path_ItemHelp_File_, Encoding.Default))
                {
                    int ObjectIndex = 0;
                    int LineCount = 0;

                    string Output = null;

                    bool IsCast = false;

                    while ((Output = Sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(Output) || Output.Substring(0, 1).Equals("#")) continue;
                        else
                        {
                            var ReadConfig = Output.Split(new char[] { ' ' });

                            if (ReadConfig == null) continue;

                            if (LineCount > 9) { LineCount = 0; continue; }

                            if (ReadConfig.Length == 1)
                            {
                                int LastIndex = ObjectIndex;

                                bool _isNumber = int.TryParse(ReadConfig[0], out ObjectIndex);

                                if (_isNumber)
                                {
                                    ItemBox.Items.Add(ObjectIndex);

                                    LineCount = 0;
                                }
                                else
                                {
                                    if (ReadConfig[0].GetIntFromHex() < 0 && ObjectIndex == 0 && LastIndex != 0)
                                    {
                                        ObjectIndex = LastIndex;

                                        goto isNotNull;
                                    }

                                    ObjectIndex = 0;
                                }

                                continue;
                            }

                            isNotNull:

                            if (ObjectIndex <= 0 || ObjectIndex >= BASE.MAX_ITEMLIST) continue;

                            g_pItemHelp[ObjectIndex].Index = ObjectIndex;

                            if (ReadConfig.Length == 2 || ReadConfig.Length == 3)
                            {
                                /* altera os "underline" da linha para espaço */

                                ReadConfig[1] = ReadConfig[1].Replace('_', ' ');

                                g_pItemHelp[ObjectIndex].AddLine(LineCount, ReadConfig[1], ReadConfig[0]);

                                LineCount++;
                            }
                        }
                    }
                }

                /* organiza a lista de itens disponíveis */

                var _arrayList = new ArrayList(ItemBox.Items);

                _arrayList.Sort();

                ItemBox.Items.Clear();

                for (int i = 0; i < _arrayList.Count; i++)
                {
                    ItemBox.Items.Add(_arrayList[i]); 
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "BASE_OpenItemHelp()");
            }
        }

        public Color GetColor(string _color)
        {
            try
            {

                return Color.FromArgb(int.Parse($"#{ _color}".Replace("#", ""), System.Globalization.NumberStyles.AllowHexSpecifier));
            }
            catch
            {

            }

            return ColorTranslator.FromHtml("#FFFFFFF");
        }

        private void ItemBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Index = 0;

            int.TryParse(ItemBox.Items[ItemBox.SelectedIndex].ToString(), out Index);

            if (Index <= 0 || Index >= BASE.MAX_ITEMLIST)
            {
                throw new ArgumentOutOfRangeException("ItemBox Out Of Range");
            }

            ClearFields();

            var Content = g_pItemHelp[Index].Line;

            C0.Text = Content[00].Color;
            M0.Text = Content[00].Message;

            L0.ForeColor = GetColor(Content[00].Color);

            C1.Text = Content[01].Color;
            M1.Text = Content[01].Message;

            L1.ForeColor = GetColor(Content[01].Color);

            C2.Text = Content[02].Color;
            M2.Text = Content[02].Message;

            L2.ForeColor = GetColor(Content[02].Color);

            C3.Text = Content[03].Color;
            M3.Text = Content[03].Message;

            L3.ForeColor = GetColor(Content[03].Color);

            C4.Text = Content[04].Color;
            M4.Text = Content[04].Message;

            L4.ForeColor = GetColor(Content[04].Color);

            C5.Text = Content[05].Color;
            M5.Text = Content[05].Message;

            L5.ForeColor = GetColor(Content[05].Color);

            C6.Text = Content[06].Color;
            M6.Text = Content[06].Message;

            L6.ForeColor = GetColor(Content[06].Color);

            C7.Text = Content[07].Color;
            M7.Text = Content[07].Message;

            L7.ForeColor = GetColor(Content[07].Color);

            C8.Text = Content[08].Color;
            M8.Text = Content[08].Message;

            L8.ForeColor = GetColor(Content[08].Color);

            C9.Text = Content[09].Color;
            M9.Text = Content[09].Message;

            L9.ForeColor = GetColor(Content[09].Color);
        }

          
        public static IEnumerable<T> GetFormControl<T>(Control _control) where T : Control
        {
            /* Testando Commint */
            var _CurControl = _control as T;  

            if (_CurControl != null) yield return _CurControl;

            var Content = _control as ContainerControl;

            if (Content != null)
            {
                foreach (Control c in Content.Controls)
                {
                    foreach (var i in GetFormControl<T>(c))
                    {
                        yield return i;

                    }
                }
            }
        }

        private void ClearFields()
        {
            foreach (var Box in GetFormControl<TextBox>(groupBox2))
            {
                Box.Text = string.Empty;
            }
        }

        private void MenuItem_OpenItemHelp(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            if (open.ShowDialog() == DialogResult.OK)
            {
                BASE_OpenItemHelp(open.FileName);
            }
        }

        private void MenuItem_SaveItemContent(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            ClearFields();

            int Index = 0;

            if (ItemBox.SelectedIndex != -1)
            {
                int.TryParse(ItemBox.Items[ItemBox.SelectedIndex].ToString(), out Index);

                var Content = g_pItemHelp[Index].Line;

                Content[00].Color = C0.Text;
                Content[00].Message = M0.Text;

                Content[01].Color = C1.Text;
                Content[01].Message = M1.Text;

                Content[02].Color = C2.Text;
                Content[02].Message = M2.Text;

                Content[03].Color = C3.Text;
                Content[03].Message = M3.Text;

                Content[04].Color = C4.Text;
                Content[04].Message = M4.Text;

                Content[05].Color = C5.Text;
                Content[05].Message = M5.Text;

                Content[06].Color = C6.Text;
                Content[06].Message = M6.Text;

                Content[07].Color = C7.Text;
                Content[07].Message = M7.Text;

                Content[08].Color = C8.Text;
                Content[08].Message = M8.Text;

                Content[09].Color = C9.Text;
                Content[09].Message = M9.Text;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (var itemhelp = new StreamWriter(save.FileName, false, Encoding.Default))
                    {
                        foreach (var item in g_pItemHelp)
                        {
                            if (item.Index == 0)
                                continue;

                            if (item.Index != 0)
                            {
                                itemhelp.WriteLine(item.Index);

                                for (int i = 0; i < 10; i++)
                                {
                                    if (item.Line[i].Color == null)
                                        item.Line[i].Color = "FFFFFFFF";

                                    if (!string.IsNullOrEmpty(item.Line[i].Message))
                                    {
                                        item.Line[i].Message = item.Line[i].Message.Replace(' ', '_');
                                    }

                                    itemhelp.WriteLine(item.Line[i].Color + " " + item.Line[i].Message);
                                }
                            }
                        }
                        itemhelp.Close();
                    }
                }
            }
        }
    }
}
