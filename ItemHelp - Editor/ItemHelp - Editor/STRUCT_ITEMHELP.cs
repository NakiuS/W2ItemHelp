using System;
using System.Globalization;

namespace W2ItemHelp
{
    public struct STRUCT_ITEMHELP
    {
        /// <summary>
        /// Indício do item.
        /// </summary>
        public int Index;

        public struct ST_HELPINFO
        {
            /// <summary>
            /// Cor da linha.
            /// </summary>
            public string Color;

            /// <summary>
            /// Texto da linha.
            /// </summary>
            public string Message;
        }

        /// <summary>
        /// Matriz que contém informações do item.
        /// </summary>
        public ST_HELPINFO[] Line;



        public bool AddLine(int _lineId, string _message, string _color)
        {
            if (0 > _lineId && _lineId >= Line.Length) return false;

            Line[_lineId].Message = _message;
            Line[_lineId].Color = _color;

            return true;
        }







        /// <summary>
        /// Método estático para criação de uma nova instância da estrutura.
        /// </summary>
        public static STRUCT_ITEMHELP CraftProperties()
        {
            var Help = new STRUCT_ITEMHELP();

            Help.Line = new ST_HELPINFO[BASE.MAX_ITEMHELP_LENGTH];

            return Help;
        }
    }
}
