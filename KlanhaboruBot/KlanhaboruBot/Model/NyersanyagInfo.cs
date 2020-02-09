using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlanhaboruBot.Model
{
    class NyersanyagInfo
    {
        private int fa;
        private int agyag;
        private int vas;
        private int raktarKapacitas;
        private int lakossag;

        public int Fa { get => fa; set => fa = value; }
        public int Agyag { get => agyag; set => agyag = value; }
        public int Vas { get => vas; set => vas = value; }
        public int RaktarKapacitas { get => raktarKapacitas; set => raktarKapacitas = value; }
        public int Lakossag { get => lakossag; set => lakossag = value; }

        public NyersanyagInfo(int fa, int agyag, int vas, int raktarKapacitas, string lakossagString)
        {
            this.fa = fa;
            this.agyag = agyag;
            this.vas = vas;
            this.raktarKapacitas = raktarKapacitas;
            lakossag = szamolLakossag(lakossagString);
        }

        private int szamolLakossag(string lakossagString)
        {
            string[] ertekek = lakossagString.Split('/');

            return Convert.ToInt32(ertekek[1]) - Convert.ToInt32(ertekek[0]);
        }

        public override string ToString()
        {
            return "fa: " + fa + "  agyag: " + agyag + "  vas: " + vas + "  raktárkapacitás: " + raktarKapacitas +
                "  lakosság: " + lakossag;
        }

    }
}
