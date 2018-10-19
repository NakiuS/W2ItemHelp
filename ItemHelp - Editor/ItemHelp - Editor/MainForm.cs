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

            InitFields();
        }

        private void InitFields()
        {
            g_pItemHelp = new STRUCT_ITEMHELP[BASE.MAX_ITEMLIST];

            for (int i = 0; i < g_pItemHelp.Length; i++)
            {
                g_pItemHelp[i] = STRUCT_ITEMHELP.CraftProperties();
            }

            BASE_OpenItemHelp();
        }

        private void BASE_OpenItemHelp()
        {
            const string _Path_ItemHelp_File_ = "ItemHelp.dat";

            try
            {
                
                using (var Sr = new StreamReader(_Path_ItemHelp_File_, Encoding.Default))
                {
                    int ObjectIndex = 0;
                    int LineCount = 0;

                    string Output = null;

                    while ((Output = Sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(Output) || Output.Substring(0, 1).Equals("#")) continue;
                        else
                        {
                            var ReadConfig = Output.Split(new char[] { ' ' });

                            if (ReadConfig == null) continue;

                            if (ReadConfig.Length == 1)
                            {
                                bool _isNumber = int.TryParse(ReadConfig[0], out ObjectIndex);

                                if (ObjectIndex >= BASE.MAX_ITEMLIST) continue;

                                if (_isNumber)
                                {
                                    ItemBox.Items.Add(ObjectIndex);

                                    LineCount = 0;
                                }
                                else
                                {
                                    ObjectIndex = 0;
                                }

                                continue;
                            }

                            if (ObjectIndex <= 0) continue;

                            g_pItemHelp[ObjectIndex].Index = ObjectIndex;

                            if (ReadConfig.Length == 2)
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
    }
}
