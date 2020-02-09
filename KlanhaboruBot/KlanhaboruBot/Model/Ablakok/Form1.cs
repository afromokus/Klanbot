using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using KlanhaboruBot.Model.Kiegeszitesek;
using KlanhaboruBot.Model;

namespace KlanhaboruBot
{

    public partial class Form1 : Form
    {
        ChromeDriver vezerlo;
        OpenQA.Selenium.IJavaScriptExecutor js;
        string sajatjelszo = "hKLVXs4arf";
        int i = 0;
        int raidhatarPercJatekos = 112;
        int raidhatarPercBotAcc = 22;
        bool elsoE = true;
        bool epulEKulcsEpulet = false;
        string fosztogatoKapacitasSzoveg;
        OpenQA.Selenium.IWebElement kezelendoElem;
        OpenQA.Selenium.IWebElement gyulekezohelyGyujtFontosElem;
        List<OpenQA.Selenium.IWebElement> vizsgalandoWebElemek = new List<OpenQA.Selenium.IWebElement>();
        NyersanyagInfo nyi;
        DateTime legutobbElkuldottCsapatokJatekos;
        DateTime legutobbElkuldottCsapatokBotAcc;

        Falu sajatFo;
        Falu botAccFo;
        Falu kezeltFalu;

        enum GyujtogetohelyElkuldott { kivalo, okos, szereny, lusta }

        public Form1()
        {
            legutobbElkuldottCsapatokJatekos = DateTime.Now.AddMinutes(-raidhatarPercJatekos);
            legutobbElkuldottCsapatokBotAcc = DateTime.Now.AddMinutes(-raidhatarPercBotAcc);
            InitializeComponent();
        }

        void kiirFileba(string szoveg, string fileNevKiterjesztes = "hami.txt")
        {
            StreamWriter stw = new StreamWriter(fileNevKiterjesztes, true);
            stw.AutoFlush = true;

            stw.Write(szoveg);

            stw.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChromeOptions lehetosegek = new ChromeOptions();
            //lehetosegek.AddArgument("--headless");
            vezerlo = new ChromeDriver(lehetosegek);
            js = (OpenQA.Selenium.IJavaScriptExecutor)vezerlo;

            vizsgalandoWebElemek = new List<OpenQA.Selenium.IWebElement>();

            vezerlo.Manage().Window.Size = new System.Drawing.Size(1280, 1024);
            vezerlo.Manage().Window.Position = new Point(0, 0);
            vezerlo.Manage().Window.Maximize();

            megadAccountErtekek();

            idozito.Start();

            idozito.Interval = 3 * 1000;
        }

        private void megnyitKlanhaboru()
        {
            vezerlo.Url = "https://www.klanhaboru.hu/";
        }

        private void RobotChrome(Falu falu)
        {
            try
            {
                bejelentkezes();

                Thread.Sleep(1000);

                vilagValasztas(kezeltFalu.Vilagszam);

                Thread.Sleep(3000);

                muveletekVegrehajtasa();

                idozito.Start();
            }
            catch//(Exception kivetel)
            {
               //MessageBox.Show(kivetel.Message);
                kijelentkezes();
                idozito.Start();
            }

            //bezarChrome();
        }

        private void megadAccountErtekek()
        {
            botAccFo = new Falu("Ezeregy", "eSDO2jlweA242", 57, 7596);

            sajatFo = new Falu("Afromokus", sajatjelszo, 59, 4043);

            kezeltFalu = new Falu("", "", 57, botAccFo.FaluSzam);
        }

        private void kijelentkezes()
        {
            vezerlo.Url = "https://www.klanhaboru.hu/page/logout";

            idozito.Start();

            /*if (jatekos == Account.botAcc)
            {
                varj(10);
            }*/

        }

        private void varj(int sec)
        {
            Thread.Sleep(sec * 1000);
        }

        private void megnyitSzobor()
        {
            vezerlo.Url = "https://hu" + kezeltFalu.Vilagszam + ".klanhaboru.hu/game.php?village=6534&screen=statue";

        }

        private void muveletekVegrehajtasa()
        {
            kezelNapiBelepesiBonusz();

            kuldetesLeadasa();

            fohadiszallasEpitInstantHaromPercAlatt(false);

            felugyelRaktarTanya();

            szoborLovagKepzes();

            kuldetesekVegrehajtasa();

            szabadCsapatokGyujtogetesreKuldese();

            //kozeliBarbarFalvakFosztogatasa();

            automatikusEpitkezesHaKell();
           
            //katonakKepzese();

            valtasKovetkezoFalura();

        }

        private void kuldetesLeadasa()
        {
            try
            {
                vezerlo.FindElementByClassName("popup_box_content").
                    FindElement(OpenQA.Selenium.By.ClassName("btn-confirm-yes")).Click();
            }
            catch
            {
            }
        }

        private void kuldetesekVegrehajtasa()
        {
            kuldetesEpites();
        }

        private void kuldetesEpites()
        {
            megnyitFohadiszallas();

            try
            {
                foreach (OpenQA.Selenium.IWebElement kuldeteselem in 
                                    vezerlo.FindElementsByClassName("current-quest"))
                {
                    try
                    {
                        kuldeteselem.Click();
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }

            letekerOldalAljara();

            try
            {
                foreach (OpenQA.Selenium.IWebElement kuldeteselem in
                                    vezerlo.FindElementsByClassName("current-quest"))
                {
                    try
                    {
                        kuldeteselem.Click();
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }

        /*private void felderitBarbarFalvak()
        {
            int x = 400;
            int y = 400;
            megnyitGyulekezohely();

            for (x = 400; x < 600; x++)
            {
                for (y = 400; y < 600; y++)
                {
                    beirCelKoordinata(x, y);
                    varj(5);
                    try
                    {
                        if (vezerlo.FindElementByClassName("village-name").Text.Contains("Barbár falu"))
                        {
                            MessageBox.Show(vezerlo.FindElementByClassName("village-item").Text);
                        }
                    }
                    catch
                    {
                        torolKoordinatak();
                    }
                    varj(5);
                }
            }
        }*/

        private void torolKoordinatak()
        {
            vezerlo.FindElementByClassName("target-input-field").SendKeys("\b\b\b\b\b\b\b");
        }

        private void valtasKovetkezoFalura()
        {
                kijelentkezes();
        }

        private void kozeliBarbarFalvakFosztogatasa()
        {
            /*if (kezeltFalu == Falu.sajatFo)
            {
                kiirFileba("Legközelebbi raidelésig hátralévő idő:\t" +
                         (raidhatarPercJatekos - (int)DateTime.Now.Subtract(legutobbElkuldottCsapatokJatekos).
                                                        TotalMinutes + "\t" + DateTime.Now + "\n\n"),
                                                                                        "RaidIdo.txt");
                if ((int)DateTime.Now.Subtract(legutobbElkuldottCsapatokJatekos).TotalMinutes >= 
                                                                                raidhatarPercJatekos)
                {
                    megnyitGyulekezohely();

                    try
                    {
                        fosztogatSajatFoFalluval();
                    }
                    catch //(Exception hiba)
                    {
                        //MessageBox.Show(hiba.Message);
                    }

                    legutobbElkuldottCsapatokJatekos = DateTime.Now;
                }
            }
            else*/ if (kezeltFalu == botAccFo)
            {
                if ((int)DateTime.Now.Subtract(legutobbElkuldottCsapatokJatekos).TotalMinutes >=
                                                                                   raidhatarPercJatekos)
                {
                    megnyitGyulekezohely();

                    try
                    {
                        fosztogatBotAccFoFaluval();
                        legutobbElkuldottCsapatokBotAcc = DateTime.Now;
                    }
                    catch
                    {

                    }
                }
            }

        }

        private void fosztogatBotAccFoFaluval()
        {
            fosztBarbarFalu(11, 428, 487);
            fosztBarbarFalu(30, 425, 486);
            fosztBarbarFalu(50, 432, 487);
            fosztBarbarFalu(50, 435, 487);
        }

        private void fosztogatSajatFoFalluval()
        {
            fosztBarbarFalu(10, 421, 489);
            fosztBarbarFalu(10, 422, 488);
            fosztBarbarFalu(10, 422, 483);
            fosztBarbarFalu(45, 420, 493);
            fosztBarbarFalu(20, 425, 486);
            fosztBarbarFalu(10, 426, 482);
            fosztBarbarFalu(30, 428, 481);
            fosztBarbarFalu(20, 420, 497);
            fosztBarbarFalu(10, 418, 502);
            fosztBarbarFalu(10, 420, 504);
            fosztBarbarFalu(20, 422, 503);
        }

        private void elkuldOsszesFelderito (int x,int y)
        {
            beirCelKoordinata(x, y);

            kijelolOsszesFelderito();

            kuldErosites();
        }

        private void kijelolOsszesFelderito()
        {
            vezerlo.FindElementById("units_entry_all_spy").Click();
        }

        private void kuldErosites()
        {
            vezerlo.FindElementById("target_support").Click();

            vezerlo.FindElementById("troop_confirm_go").Click();
        }

        private void foglalFalu(int x, int y)
        {
            megnyitGyulekezohely();

            beirCelKoordinata(x, y);

            kijelolOsszesEgyseg();

            int fonemesekSzama = 0;

            switch (vezerlo.FindElementById("unit_input_snob").GetAttribute("data-all-count"))
            {
                case "":
                    fonemesekSzama = 0;
                    break;
                case "1": fonemesekSzama = 1;
                    break;
            }

            if (fonemesekSzama == 1)
            {
                tamadokKuldese();
            }
            /*else
            {
                MessageBox.Show("A főnemes nem tartózkodik a faluban.");
            }*/
            
        }

        private void beirCelKoordinata(int x, int y)
        {
            /*js.ExecuteScript("document.getElementsByClassName(\"target-input-field\")[0]." +
                   "setAttribute(\"value\", \"" + x + "|" + y + "\")");*/

            vezerlo.FindElementByClassName("target-input-field").SendKeys("" + x + "|" + y);
        }

        private void fosztBarbarFalu(int dbKonnyuLovas, int x, int y, bool kuldEOsszesFaltoroKos = false)
        {
            js.ExecuteScript("document.getElementById(\"unit_input_light\").setAttribute(\"value\", " +
                "                                                          \"" + dbKonnyuLovas + "\")");

            beirCelKoordinata(x, y);


            if (kuldEOsszesFaltoroKos)
            {
                try
                {
                    vezerlo.FindElementById("units_entry_all_ram").Click();
                }
                catch
                {

                }
            }

            try
            {
                System.Threading.Thread.Sleep(100);
                if (vezerlo.FindElementByClassName("village-name").Text.Contains("Barbár falu"))
                {
                    tamadokKuldese();
                }
                else
                {
                    torolKoordinatak();
                }
            }
            catch
            {

            }

        }

        private void tamadokKuldese()
        {
            vezerlo.FindElementById("target_attack").Click();

            vezerlo.FindElementById("troop_confirm_go").Click();
        }

        private void kezelNapiBelepesiBonusz()
        {
            if (kezeltFalu == botAccFo || kezeltFalu == sajatFo)
            {
                try
                {
                    vezerlo.FindElementsByTagName("a").Last(l => l.Text.Contains("Kinyit")).Click();

                    if (kezeltFalu == botAccFo)
                    {
                        kiirFileba("Robot napi bónusz beváltva ekkor:\t" + DateTime.Now, "RobotBonusz.txt");
                    }
                    else if (kezeltFalu == sajatFo)
                    {
                        kiirFileba("Afromokus napi bónusz beváltva ekkor:\t" + DateTime.Now, "SajatBonuszIdo.txt");
                    }

                }
                catch //(Exception hiba)
                {
                    //MessageBox.Show(hiba.Message);
                }
            }

        }

        private void felugyelRaktarTanya()
        {
            epulEKulcsEpulet = false;
            try
            {
                if (vezerlo.FindElementById("build_queue").Text.Contains("Raktár"))
                {
                    epulEKulcsEpulet = true;
                }
            }
            catch
            {
                epulEKulcsEpulet = false;
            }

            nyi = lekerEroforrasok();

            int legnagyobbLakossagIgeny = lekerLegnagyobbLakossagigeny();

            //MessageBox.Show(legnagyobbLakossagIgeny + "");

            if (nyi.Fa >= nyi.RaktarKapacitas - 200 || nyi.Agyag >= nyi.RaktarKapacitas ||
                                        nyi.Vas >= nyi.RaktarKapacitas)
            {
                try
                {
                    if (!epulEKulcsEpulet)
                    {
                        epitRaktar();
                        epulEKulcsEpulet = false;
                    }
                }
                catch
                {
                    //MessageBox.Show("Hiányzó nyersanyag.");
                }
            }

            try
            {
                if (vezerlo.FindElementById("build_queue").Text.Contains("Tanya"))
                {
                    epulEKulcsEpulet = true;
                }
            }
            catch
            {
                epulEKulcsEpulet = false;
            }

            if (nyi.Lakossag <= legnagyobbLakossagIgeny * 5)
            {
                try
                {
                    if (!epulEKulcsEpulet)
                    {
                        letekerOldalAljara();
                        epitTanya();
                        epulEKulcsEpulet = false;
                    }
                }
                catch
                {
                    //MessageBox.Show("Nincs elég nyersanyag a tanya építéséhez.");
                }
            }

        }

        private int lekerLegnagyobbLakossagigeny()
        {
            int legnagyobbLakossagIgeny = 0;
            string[] elemek;

            //főhadiszállás
            elemek = vezerlo.FindElementById("main_buildrow_main").getInnerHTML().
                                Split(new string[] { "/span>" }, StringSplitOptions.None);

            legnagyobbLakossagIgeny = Convert.ToInt32(elemek[6].Split('<')[0]);

            //agyagbánya
            elemek = vezerlo.FindElementById("main_buildrow_stone").getInnerHTML().
                                Split(new string[] { "/span>" }, StringSplitOptions.None);

            if (legnagyobbLakossagIgeny < Convert.ToInt32(elemek[6].Split('<')[0]))
            {
                legnagyobbLakossagIgeny = Convert.ToInt32(elemek[6].Split('<')[0]);
            }

            //fatelep
            elemek = vezerlo.FindElementById("main_buildrow_wood").getInnerHTML().
                                Split(new string[] { "/span>" }, StringSplitOptions.None);

            if (legnagyobbLakossagIgeny < Convert.ToInt32(elemek[6].Split('<')[0]))
            {
                legnagyobbLakossagIgeny = Convert.ToInt32(elemek[6].Split('<')[0]);
            }

            //vasbánya
            elemek = vezerlo.FindElementById("main_buildrow_iron").getInnerHTML().
                                Split(new string[] { "/span>" }, StringSplitOptions.None);

            if (legnagyobbLakossagIgeny < Convert.ToInt32(elemek[6].Split('<')[0]))
            {
                legnagyobbLakossagIgeny = Convert.ToInt32(elemek[6].Split('<')[0]);
            }

            return legnagyobbLakossagIgeny;
        }

        private NyersanyagInfo lekerEroforrasok()
        {
            int fa = 0;
            int agyag = 0;
            int vas = 0;
            int raktarKapacitas = 0;
            string lakossagString = "";


            fa = Convert.ToInt32(vezerlo.FindElementByClassName("smallPadding").
                                FindElements(OpenQA.Selenium.By.ClassName("box-item"))[1].Text);

            agyag = Convert.ToInt32(vezerlo.FindElementByClassName("smallPadding").
                                FindElements(OpenQA.Selenium.By.ClassName("box-item"))[3].Text);

            vas = Convert.ToInt32(vezerlo.FindElementByClassName("smallPadding").
                                FindElements(OpenQA.Selenium.By.ClassName("box-item"))[5].Text);

            raktarKapacitas = Convert.ToInt32(vezerlo.FindElementByClassName("smallPadding").
                                FindElements(OpenQA.Selenium.By.ClassName("box-item"))[7].Text);

            lakossagString = vezerlo.FindElementByClassName("smallPadding").
                                FindElements(OpenQA.Selenium.By.ClassName("box-item"))[9].Text;

            NyersanyagInfo nyi = new NyersanyagInfo(fa, agyag, vas, raktarKapacitas, lakossagString);

            //MessageBox.Show(nyi + "");

            return nyi;
        }

        private void katonakKepzese()
        {
            if (kezeltFalu == botAccFo)
            {
                try
                {
                    megnyitBarakk();
                    letekerOldalAljara();
                    kepezKardforgato(1);
                }
                catch
                {
                    //MessageBox.Show("Lándzsás képzéséhez nincs elég nyersanyag.");
                }

                visszateresFaluAttekintesre();
            }
            /*else if (kezeltFalu == Falu.sajatFo)
            {
                if (nyi.Fa > nyi.Agyag * 2 && nyi.Vas > nyi.Agyag * 2)
                {
                    megnyitIstallo();
                    letekerOldalAljara();
                    kepezKonnyuLovas(1);
                    megnyitBarakk();
                    letekerOldalAljara();
                    kepezLandzsas(3);
                }
            }*/

        }

        private void KepezLovasIjasz(int egysegekSzama)
        {
            beirLovasIjasz(egysegekSzama);

            vezerlo.FindElementByClassName("btn-recruit").Click();
        }

        private void beirLovasIjasz(int egysegekSzama)
        {
            //marcher_0
            js.ExecuteScript("document.getElementById(\"marcher_0\")." +
                "value=\"" + egysegekSzama + "\"");
        }

        private void kepezFaVas()
        {
            megnyitBarakk();
            letekerOldalAljara();
            kepezLandzsas(6);

            megnyitIstallo();
            letekerOldalAljara();
            kepezKonnyuLovas(2);
        }

        private void kepezMuhelyAgyagEgyelo()
        {
            try
            {
                if (nyi.Fa >= 320)
                {
                    megnyitMuhely();
                    letekerOldalAljara();
                    kepezKatapult(1);

                    megnyitBarakk();
                    letekerOldalAljara();
                    kepezLandzsas(3);
                }
            }
            catch
            {

            }

            megnyitBarakk();
            letekerOldalAljara();
            kepezKardforgato(9);
        }

        private void kepezKardforgato(int egysegekSzama)
        {
            //sword_0
            beirKardforgato(egysegekSzama);

            vezerlo.FindElementByClassName("btn-recruit").Click();
        }

        private void beirKardforgato(int egysegekSzama)
        {
            js.ExecuteScript("document.getElementById(\"sword_0\")." +
                       "value=\"" + egysegekSzama + "\"");
        }

        private void megnyitMuhely()
        {
            vezerlo.Url = "https://hu" + kezeltFalu.Vilagszam + ".klanhaboru.hu/game.php?village=7104&screen=garage";
        }

        private void kepezKatapult(int db)
        {
            js.ExecuteScript("document.getElementById(\"catapult_0\")." +
                       "value=\"" + db + "\"");

            vezerlo.FindElementByClassName("btn-recruit").Click();
        }

        private void kepezKonnyuLovas(int egysegekSzama)
        {
            //light_0
            try
            {
                js.ExecuteScript("document.getElementById(\"light_0\")." +
                    "value=\"" + egysegekSzama + "\"");
            }
            catch
            {
            }

            vezerlo.FindElementByClassName("btn-recruit").Click();

        }

        private void megnyitIstallo()
        {
            vezerlo.Url = "https://hu" + kezeltFalu.Vilagszam + ".klanhaboru.hu/game.php?village=7121&screen=stable";
        }

        private void megnyitBarakk()
        {
            vezerlo.Url = "https://hu" + kezeltFalu.Vilagszam + ".klanhaboru.hu/game.php?village=7121&screen=barracks";
        }

        private void kepezLandzsas(int egysegekSzama)
        {
            //spear_0
            try
            {
                beirLandzsas(egysegekSzama);
            }
            catch
            {
            }

            vezerlo.FindElementByClassName("btn-recruit").Click();
        }

        private void beirLandzsas(int egysegekSzama)
        {
            js.ExecuteScript("document.getElementById(\"spear_0\")." +
                       "value=\"" + egysegekSzama + "\"");
        }

        private void osszesLandzsasKepzese()
        {
            vezerlo.FindElementById("spear_0_a").Click();
            vezerlo.FindElementByClassName("btn-recruit").Click();
        }

        private void automatikusEpitkezesHaKell()
        {
            if (kezeltFalu.FaluSzam == botAccFo.FaluSzam)
            {

                megnyitFohadiszallas();

                try
                {
                    epitesPrioritasSzerint();
                }
                catch //(Exception hiba)
                {
                    //MessageBox.Show(hiba.Message);
                }

            }
            /*else if (jatekos == Account.sajat)
            {
                megnyitFohadiszallas();

                try
                {
                    epitKovacsMuhely();
                }
                catch
                {

                }

            }*/


        }

        private void epitesPrioritasSzerint()
        {
            try
            {
                epitFohadiszallas();
            }
            catch
            {
                try
                {
                    letekerOldalAljara();
                    epitAgyagbanya();
                }
                catch
                {
                    try
                    {
                        epitFatelep();
                    }
                    catch
                    {
                        try
                        {
                            epitVasbanya();
                        }
                        catch
                        {
                            if (kezeltFalu == botAccFo)
                            {
                                try
                                {
                                    feltekerOldalTetejere();
                                    epitKovacsMuhely();
                                }
                                catch
                                {
                                    try
                                    {
                                        letekerOldalAljara();
                                        epitFal();
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            else if (kezeltFalu == sajatFo)
                            {
                                epitFal();
                            }
                        }
                    }

                }
            }

        }

        private void epitIstallo()
        {
            vezerlo.FindElementById("main_buildrow_stable").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void epitBarakk()
        {
            vezerlo.FindElementById("main_buildrow_barracks").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void feltekerOldalTetejere()
        {
            js.ExecuteScript("window.scrollTo(0, 0)");
        }

        private void epitKovacsMuhely()
        {            
            vezerlo.FindElementById("main_buildrow_smith").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void epitFal()
        {
            vezerlo.FindElementById("main_buildrow_wall").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void epitRaktar()
        {
            vezerlo.FindElementById("main_buildrow_storage").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void epitTanya()
        {
            vezerlo.FindElementById("main_buildrow_farm").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void epitVasbanya()
        {
            vezerlo.FindElementById("main_buildrow_iron").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void epitFatelep()
        {
            vezerlo.FindElementById("main_buildrow_wood").
                            FindElements(OpenQA.Selenium.By.TagName("a")).
                                    First(l => l.Text.Contains(". szint")).Click();
        }

        private void epitAgyagbanya()
        {
            vezerlo.FindElementById("main_buildrow_stone").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void epitFohadiszallas()
        {
            vezerlo.FindElementById("main_buildrow_main").
                                FindElements(OpenQA.Selenium.By.TagName("a")).
                                        First(l => l.Text.Contains(". szint")).Click();
        }

        private void szoborLovagKepzes()
        {

            megnyitSzobor();

            lovagKinevezese("Jonas");

            try
            {
                lovagKepzes();
            }
            catch
            {
                //MessageBox.Show("A lovag már edzésen van.");
            }

        }

        private void lovagKinevezese(string nev)
        {
            try
            {
                vezerlo.FindElementByClassName("knight_recruit_launch").Click();
                varj(5);
                elnevezLovagot(nev);
                vezerlo.FindElementById("knight_recruit_confirm").Click();

            }
            catch
            {

            }

        }

        private void elnevezLovagot(string nev)
        {

            //knight_recruit_name
            js.ExecuteScript("document.getElementById(\"knight_recruit_name\")." +
                                "setAttribute(\"value\", \"" + nev + "\")");

        }

        private void lovagKepzes()
        {
            vezerlo.FindElementByClassName("knight_train_launch").Click();

            vezerlo.FindElementsByTagName("a").First(l => l.Text == "Start").Click();
        }

        private void szabadCsapatokGyujtogetesreKuldese()
        {
                gyujtogetes();
        }

        private void gyujtogetes()
        {
            megnyitGyujtogetes();

            /* if (jatekos == Account.botAcc)
             {
                 try
                 {
                     vezerlo.FindElementsByTagName("a").First(l => l.getInnerHTML().Contains("Kinyit")).Click();
                     vezerlo.FindElementsByTagName("a").Last(l => l.getInnerHTML().Contains("Kinyit")).Click();
                 }
                 catch(Exception hiba)
                 {
                     MessageBox.Show(hiba.Message);
                 }

                 megnyitGyujtogetes();
             }*/


            kattintlLandzsasok();

            gyulekezohelyGyujtFontosElem = vezerlo.FindElementByClassName("carry-max");

            fosztogatoKapacitasSzoveg = gyulekezohelyGyujtFontosElem.getInnerHTML();

            if (atalakitSzamma(fosztogatoKapacitasSzoveg) > 0)
            {
                try
                {
                    csapatokKuldeseKivaloGyujtogetok();
                }
                catch
                {
                }
            }
            else
            {
                //MessageBox.Show("Nincsenek szabad katonák.");
            }

            tobbiekElkuldese();

        }

        private void tobbiekElkuldese()
        {
            //MessageBox.Show("többiek küldése");
            try
            {
                feltekerOldalTetejere();
                //összes egység
                kijelolOsszesEgyseg();

                //kivéve
                kattintlLandzsasok();

                kattintKonnyuLovassag();

                kattintNehezLovassag();

                gyulekezohelyGyujtFontosElem = vezerlo.FindElementByClassName("carry-max");

                fosztogatoKapacitasSzoveg = gyulekezohelyGyujtFontosElem.getInnerHTML();
                //MessageBox.Show("elküldés");

                if (atalakitSzamma(fosztogatoKapacitasSzoveg) > 0)
                {
                    csapatokKuldeseOkosGyujtogetok();

                }
                else
                {
                    //MessageBox.Show("Nincsenek szabad katonák.");
                }
            }
            catch //(Exception hiba)
            {
                //MessageBox.Show(hiba.Message);
            }

        }

        private void kattintNehezLovassag()
        {
            //Nehézlovasság
            gyulekezohelyGyujtFontosElem = vezerlo.FindElementsByClassName("units-entry-all").
                                                    First(l => l.GetAttribute("data-unit") == "heavy");

            gyulekezohelyGyujtFontosElem.Click();
        }

        private void csapatokKuldeseOkosGyujtogetok()
        {

            vezerlo.FindElementsByClassName("scavenge-option")[2].
                                            FindElement(OpenQA.Selenium.By.TagName("a")).Click();
        }

        private void kattintKonnyuLovassag()
        {
            //Könnyűlovasok
            gyulekezohelyGyujtFontosElem = vezerlo.FindElementsByClassName("units-entry-all").
                                                    First(l => l.GetAttribute("data-unit") == "light");

            gyulekezohelyGyujtFontosElem.Click();
        }

        private void csapatokKuldeseSzerenyGyujtogetok()
        {
            vezerlo.FindElementsByClassName("scavenge-option")[1].
                                            FindElement(OpenQA.Selenium.By.TagName("a")).Click();
        }

        private void csapatokKuldeseLegjobbHelyre()
        {
            
            try
            {
                csapatokKuldeseKivaloGyujtogetok();
            }
            catch
            {
                feltekerOldalTetejere();
                vezerlo.FindElementsByClassName("scavenge-option")[2].
                                                FindElement(OpenQA.Selenium.By.TagName("a")).Click();

            }
            finally
            {
               //MessageBox.Show("Többiek küldése");
                tobbiekElkuldese();
            }

        }

        private void csapatokKuldeseKivaloGyujtogetok()
        {
            letekerOldalAljara();
            vezerlo.FindElementsByClassName("scavenge-option")[3].
                   FindElements(OpenQA.Selenium.By.TagName("a")).First(l => l.getInnerHTML().
                                                                         Contains("Kezdés")).Click();
        }

        private void letekerOldalAljara()
        {
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }

        private void kattintlLandzsasok()
        {
            //lándzsások
            gyulekezohelyGyujtFontosElem = vezerlo.FindElementsByClassName("units-entry-all").
                                                        First(l => l.GetAttribute("data-unit") == "spear");

            gyulekezohelyGyujtFontosElem.Click();
        }

        private void kijelolOsszesEgyseg()
        {
            gyulekezohelyGyujtFontosElem = vezerlo.FindElementsByTagName("a").
                                                            First(l => l.getInnerHTML().Contains("Összes csapat"));
            gyulekezohelyGyujtFontosElem.Click();
        }

        private void visszateresFaluAttekintesre()
        {
            if (vezerlo.Url != "https://hu" + kezeltFalu.Vilagszam + ".klanhaboru.hu/game.php?screen=overview&intro")
            {
                vezerlo.Url = "https://hu" + kezeltFalu.Vilagszam + ".klanhaboru.hu/game.php?screen=overview&intro";
            }
        }

        private int atalakitSzamma(string fosztogatoKapacitasSzoveg)
        {
            string ertek = "";

            foreach (char c in fosztogatoKapacitasSzoveg)
            {
                if (int.TryParse(c.ToString(), out i))
                {
                    ertek += "" + i;
                }
            }

            i = 0;

            return Convert.ToInt32(ertek);
        }

        private void megnyitGyujtogetes()
        {
            visszateresFaluAttekintesre();

            megnyitGyulekezohely();

            foreach (OpenQA.Selenium.IWebElement link in vezerlo.FindElementByClassName("modemenu").
                                                                       FindElements(OpenQA.Selenium.By.TagName("a")))
            {
                if (i == 2)
                {
                    gyulekezohelyGyujtFontosElem = link;
                }
                i++;
            }

            i = 0;

            gyulekezohelyGyujtFontosElem.Click();
        }

        private void megnyitGyulekezohely()
        {

            try
            {
                vezerlo.Url = "https://hu" + kezeltFalu.Vilagszam + ".klanhaboru.hu/game.php?village=6534&screen=place";
            }
            catch
            {
            }

        }

        private void csapatokKuldese()
        {

            int szamolZaroltGyujtesek = 0;

            vizsgalandoWebElemek = feltoltVizsgalandoWebElemekOsztNevAlapjan("options-container");
            vizsgalandoWebElemek.Reverse();

            foreach (OpenQA.Selenium.IWebElement elem in vizsgalandoWebElemek)
            {
                if (elem.getInnerHTML().Contains("Kinyit"))
                {
                    szamolZaroltGyujtesek++;
                }
            }
            try
            {
                vizsgalandoWebElemek[szamolZaroltGyujtesek + 1].Click();
            }
            catch//(Exception hiba)
            {
                //MessageBox.Show(hiba.Message);
            }
        }

        private List<OpenQA.Selenium.IWebElement> feltoltVizsgalandoWebElemekOsztNevAlapjan(string osztalyNev)
        {
            List<OpenQA.Selenium.IWebElement> elemek = new List<OpenQA.Selenium.IWebElement>();

            foreach (OpenQA.Selenium.IWebElement link in vezerlo.FindElementByClassName(osztalyNev).
                                                                         FindElements(OpenQA.Selenium.By.TagName("a")))
            {
                elemek.Add(link);
            }

            return elemek;
        }

        private void fohadiszallasEpitInstantHaromPercAlatt(bool okosE)
        {
            megnyitFohadiszallas();

            epitHaromPercAlatt();

        }

        private void epitHaromPercAlatt()
        {
            try
            {
                vezerlo.FindElementByClassName("btn-instant-free").Click();
                varj(4);
                kuldetesLeadasa();

            }
            catch (Exception hiba)
            {

                if (hiba.Message.Contains("no such element"))
                {
                    //MessageBox.Show("Jelenleg nem zajlik építkezés.");
                }
                else if(hiba.Message.Contains("not interactable"))
                {
                    //MessageBox.Show("Még nem vagyunk 3 perc alatt");
                }

            }
        }

        private int vizsgalIdo(string idoString)
        {
            string[] elemek = idoString.Split(':');

            return Convert.ToInt16(elemek[0]) * 60 * 60 + Convert.ToInt16(elemek[1]) * 60 + 
                                                                        Convert.ToInt16(elemek[2]);
        }

        private void megnyitFohadiszallas()
        {
            try
            {
                visszateresFaluAttekintesre();

                vezerlo.Url = "https://hu" + kezeltFalu.Vilagszam + ".klanhaboru.hu/game.php?village=" +
                    kezeltFalu.FaluSzam + "&screen=main";

                if (!vezerlo.Url.Contains("&screen=main"))
                {
                    megnyitFohadiszallas();
                    //MessageBox.Show("Nehézség a főhadiszállással");
                }

                if (vezerlo.Url.Contains("7121") && kezeltFalu == sajatFo)
                {
                    kijelentkezes();
                    RobotChrome(sajatFo);
                }
                else if (vezerlo.Url.Contains("7104") && kezeltFalu == botAccFo)
                {
                    kijelentkezes();
                    RobotChrome(botAccFo);
                }
            }
            catch
            {
            }

        }

        private void vilagValasztas(int vilag)
        {

            try
            {
                kezelendoElem = vezerlo.FindElementsByClassName("world_button_active").
                                             First(l => l.Text.Contains(kezeltFalu.Vilagszam + ""));
                kezelendoElem.Click();
            }
            catch
            {
            }
        }

        private void bejelentkezes()
        {
            idozito.Stop();
            megnyitKlanhaboru();

            if (elsoE)
            {

                if (vezerlo.FindElementByName("remember-me").GetAttribute("checked") == "true")
                {
                    vezerlo.FindElementByClassName("remember_me").Click();
                }

            }

            varj(1);

            js.ExecuteScript("document.getElementById(\"user\").setAttribute(\"value\", \"" + 
                kezeltFalu.FelhNev + "\")");
            js.ExecuteScript("document.getElementById(\"password\").setAttribute(\"value\", \"" + 
                kezeltFalu.Jelszo + "\")");

            vezerlo.FindElementByClassName("btn-login").Click();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                bezarChrome();
            }

        }

        private void bezarChrome()
        {
            vezerlo.Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            vezerlo.Quit();
        }

        private void idozito_Tick(object sender, EventArgs e)
        {
            idozitoEsemeny();
        }

        private void idozitoEsemeny()
        {
            this.Hide();

            try
            {
                if (kezeltFalu.FaluSzam == botAccFo.FaluSzam)
                {
                    kezeltFalu = sajatFo;
                }
                else if (kezeltFalu.FaluSzam == sajatFo.FaluSzam)
                {
                    kezeltFalu = botAccFo;
                }

                RobotChrome(kezeltFalu);

                ebrentartSzamitogepet();
            }
            catch (Exception hiba)
            {
                System.Diagnostics.Debug.Write(hiba.Message + "\t" + DateTime.Now.ToLongDateString());
            }
        }

        private void ebrentartSzamitogepet()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
        }

        [FlagsAttribute()]
        public enum EXECUTION_STATE : uint //Determine Monitor State
        {
            ES_AWAYMODE_REQUIRED = 0x40,
            ES_CONTINUOUS = 0x80000000u,
            ES_DISPLAY_REQUIRED = 0x2,
            ES_SYSTEM_REQUIRED = 0x1
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        //Enables an application to inform the system that it is in use, thereby preventing the system from entering sleep or turning off the display while the application is running.
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        //This function queries or sets system-wide parameters, and updates the user profile during the process.
        [DllImport("user32", EntryPoint = "SystemParametersInfo", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const Int32 SPI_SETSCREENSAVETIMEOUT = 15;
    }
}
