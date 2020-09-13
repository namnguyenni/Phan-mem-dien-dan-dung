namespace ESCS
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    [Serializable]
    public class Elements : Control
    {
        public ArrayList alElements;
        private Timer alertTimer;
        public ArrayList alWay;
        private float angle;
        private bool bHand;
        public bool bModifyWay;
        private IContainer components;
        private float fDegree;
        private Hashtable htSysState;
        private Hashtable htSysVars;
        public int iGroup;
        public int int1;
        public int int2;
        private int iRotation;
        public Label label1;
        public Label label2;
        public static int maxType = 0x1b;
        private int mCnt;
        public Color mColor;
        private Size mContSize;
        private CPoint[] mCPoint;
        public int miRefresPeriod;
        private Point mLabel2Loc;
        private Point mLabelLoc;
        private bool mMovable;
        private Point moPoint;
        private bool mSelected;
        private int mState;
        private int mStateCount;
        private ElementType mType;
        private Point pShift;
        private float r;
        private Timer scanTimer;
        private Timer timer1;

        public Elements()
        {
            this.bHand = false;
            this.iRotation = 0;
            this.InitializeComponent();
            this.Initialize2();
        }

        public Elements(ElementType e)
        {
            this.bHand = false;
            this.iRotation = 0;
            this.InitializeComponent();
            this.Initialize2();
            this.Type = e;
        }

        private Point addPoint(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        private void alertTimer_Tick(object sender, EventArgs e)
        {
            if (this.BackColor != Color.Red)
            {
                this.BackColor = Color.Red;
            }
            else
            {
                this.BackColor = Color.Yellow;
            }
        }

        public bool AnalizeState()
        {
            switch (this.Type)
            {
                case ElementType.Kapi1:
                case ElementType.Kapi2:
                case ElementType.Asansor:
                {
                    int num = ((frmMain) base.Parent.Parent).c1Motor3FazControl(new CPoint[] { this.mCPoint[0], this.mCPoint[1], this.mCPoint[2] });
                    switch (num)
                    {
                        case 0:
                            this.htSysStateSet("p1", false);
                            this.changeCPState();
                            this.mCnt = 0;
                            goto Label_0175;

                        case 1:
                        case 2:
                            this.int1 = (num == 1) ? 1 : -1;
                            if (this.Type == ElementType.Asansor)
                            {
                                ((int[]) this.htSysVars["asansor"])[4] = this.int1;
                            }
                            else
                            {
                                ((int[]) this.htSysVars["kapi"])[4] = this.int1;
                            }
                            this.htSysStateSet("p1", true);
                            this.changeCPState();
                            ((frmMain) base.Parent.Parent).c1ActivateWay(this.mCPoint[0].way);
                            ((frmMain) base.Parent.Parent).c1ActivateWay(this.mCPoint[1].way);
                            ((frmMain) base.Parent.Parent).c1ActivateWay(this.mCPoint[2].way);
                            this.modifyWay = true;
                            goto Label_0175;

                        case 3:
                            return false;
                    }
                    break;
                }
            }
        Label_0175:
            return true;
        }

        public void ChangeContacts(int iState)
        {
            foreach (Elements elements in this.alElements)
            {
                elements.State = iState;
            }
        }

        private void ChangeContactsCounter(string count)
        {
            foreach (Elements elements in this.alElements)
            {
                elements.label2.Text = count;
            }
            this.label2.Text = count;
        }

        public void changeCPState()
        {
            switch (this.Type)
            {
                case ElementType.Kapi1:
                case ElementType.Kapi2:
                    if (!((bool) this.htSysState["p2"]))
                    {
                        this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[6] };
                        this.mCPoint[4].arrCP = new CPoint[0];
                        this.mCPoint[5].arrCP = new CPoint[0];
                        this.mCPoint[6].arrCP = new CPoint[] { this.mCPoint[3] };
                        break;
                    }
                    this.mCPoint[3].arrCP = new CPoint[0];
                    this.mCPoint[4].arrCP = new CPoint[] { this.mCPoint[5] };
                    this.mCPoint[5].arrCP = new CPoint[] { this.mCPoint[4] };
                    this.mCPoint[6].arrCP = new CPoint[0];
                    break;

                case ElementType.Asansor:
                    for (int i = 2; i < 7; i++)
                    {
                        if ((bool) this.htSysState["p" + i])
                        {
                            this.mCPoint[3 + ((i - 2) * 4)].arrCP = new CPoint[0];
                            this.mCPoint[4 + ((i - 2) * 4)].arrCP = new CPoint[] { this.mCPoint[5 + ((i - 2) * 4)] };
                            this.mCPoint[5 + ((i - 2) * 4)].arrCP = new CPoint[] { this.mCPoint[4 + ((i - 2) * 4)] };
                            this.mCPoint[6 + ((i - 2) * 4)].arrCP = new CPoint[0];
                        }
                        else
                        {
                            this.mCPoint[3 + ((i - 2) * 4)].arrCP = new CPoint[] { this.mCPoint[6 + ((i - 2) * 4)] };
                            this.mCPoint[4 + ((i - 2) * 4)].arrCP = new CPoint[0];
                            this.mCPoint[5 + ((i - 2) * 4)].arrCP = new CPoint[0];
                            this.mCPoint[6 + ((i - 2) * 4)].arrCP = new CPoint[] { this.mCPoint[3 + ((i - 2) * 4)] };
                        }
                    }
                    this.timer1.Enabled = (bool) this.htSysState["p1"];
                    this.frmMain_c1ScanCircuit();
                    return;

                default:
                    MessageBox.Show("no function for this type:" + this.Type.ToString() + " Elements.changeCPState()");
                    return;
            }
            if ((bool) this.htSysState["p3"])
            {
                this.mCPoint[7].arrCP = new CPoint[0];
                this.mCPoint[8].arrCP = new CPoint[] { this.mCPoint[9] };
                this.mCPoint[9].arrCP = new CPoint[] { this.mCPoint[8] };
                this.mCPoint[10].arrCP = new CPoint[0];
            }
            else
            {
                this.mCPoint[7].arrCP = new CPoint[] { this.mCPoint[10] };
                this.mCPoint[8].arrCP = new CPoint[0];
                this.mCPoint[9].arrCP = new CPoint[0];
                this.mCPoint[10].arrCP = new CPoint[] { this.mCPoint[7] };
            }
            this.timer1.Enabled = (bool) this.htSysState["p1"];
            this.frmMain_c1ScanCircuit();
        }

        private void Elements_MouseDown(object sender, MouseEventArgs e)
        {
            base.Focus();
            if (this.State == 0)
            {
                if (e.Button != MouseButtons.Right)
                {
                    this.oPoint = new Point(e.X, e.Y);
                    this.mLabelLoc = new Point(this.label1.Location.X - base.Location.X, this.label1.Location.Y - base.Location.Y);
                    if (this.label2.Enabled)
                    {
                        this.mLabel2Loc = new Point(this.label2.Location.X - base.Location.X, this.label2.Location.Y - base.Location.Y);
                    }
                }
            }
            else
            {
                switch (this.Type)
                {
                    case ElementType.Role:
                    case ElementType.StartButon:
                    case ElementType.StopButon:
                    case ElementType.StartStopButon:
                    case ElementType.SinirAnahtar:
                    case ElementType.Startx3:
                    case ElementType.Start3x3:
                    case ElementType.SinirAnahtar2:
                        if (e.Button != MouseButtons.Right)
                        {
                            this.State = 2;
                            return;
                        }
                        if (this.State == 1)
                        {
                            this.State = 2;
                            return;
                        }
                        this.State = 1;
                        return;

                    case ElementType.ZamanRolesi:
                    case ElementType.TersZamanRolesi:
                    case ElementType.AcikKontak:
                    case ElementType.KapaliKontak:
                        break;

                    default:
                        return;
                }
            }
        }

        private void Elements_MouseMove(object sender, MouseEventArgs e)
        {
            if ((this.Selected && (this.State == 0)) && (e.Button == MouseButtons.Left))
            {
                this.Elements_Move(new Point(e.X - this.oPoint.X, e.Y - this.oPoint.Y));
                this.miRefresPeriod++;
                if (this.miRefresPeriod > 20)
                {
                    this.miRefresPeriod = 0;
                    this.Refresh();
                }
            }
        }

        private void Elements_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.State != 0)
            {
                switch (this.Type)
                {
                    case ElementType.Role:
                        this.frmMain_c1ScanCircuit();
                        return;

                    case ElementType.ZamanRolesi:
                    case ElementType.TersZamanRolesi:
                    case ElementType.AcikKontak:
                    case ElementType.KapaliKontak:
                        return;

                    case ElementType.StartButon:
                    case ElementType.StopButon:
                    case ElementType.StartStopButon:
                    case ElementType.SinirAnahtar:
                    case ElementType.Startx3:
                    case ElementType.Start3x3:
                    case ElementType.SinirAnahtar2:
                        if (e.Button != MouseButtons.Right)
                        {
                            this.State = 1;
                        }
                        return;
                }
            }
            else
            {
                this.oPoint = new Point(0, 0);
            }
        }

        public void Elements_Move(Point e)
        {
            base.Location = this.addPoint(base.Location, e);
            this.label1.Location = this.addPoint(this.label1.Location, e);
            if (this.label2.Enabled)
            {
                this.label2.Location = this.addPoint(this.label2.Location, e);
            }
        }

        private void Elements_VisibleChanged(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                this.label1.Visible = true;
                if (this.label2.Enabled)
                {
                    this.label2.Visible = true;
                }
            }
            else
            {
                this.label1.Visible = false;
                if (this.label2.Enabled)
                {
                    this.label2.Visible = false;
                }
            }
        }

        private void frmMain_c1AlertElement(int type)
        {
            if (((base.Parent != null) && (base.Parent.Parent != null)) && (base.Parent.Parent.GetType() == typeof(frmMain)))
            {
                ((frmMain) base.Parent.Parent).c1AlertElement(this, type);
            }
        }

        private void frmMain_c1ScanCircuit()
        {
            try
            {
                ((frmMain) base.Parent.Parent).c1ScanCircuit();
            }
            catch (Exception exception)
            {
                Trace.WriteLine("hata: gercek fonksiyon bulunamadi! 'Elements.frmMain_c1ScanCircuit()' " + exception.ToString());
            }
        }

        private void frmMain_Reconnect(CPoint[] cp1, CPoint[] cp2)
        {
            if (((base.Parent != null) && (base.Parent.Parent != null)) && (base.Parent.Parent.GetType() == typeof(frmMain)))
            {
                ((frmMain) base.Parent.Parent).ReconnectCP(cp1, cp2);
            }
        }

        public void htSysStateSet(string key, bool val)
        {
            if (((bool) this.htSysState[key]) != val)
            {
                this.htSysState[key] = val;
                this.Refresh();
            }
        }

        private void InitCPoint()
        {
            CPoint[] mCPoint = this.mCPoint;
            switch (this.Type)
            {
                case ElementType.Role:
                case ElementType.ZamanRolesi:
                case ElementType.TersZamanRolesi:
                case ElementType.Lamba:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 10)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 10)), this.RelSide(CPoint.Sides.Right)) };
                    this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                    this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                    break;

                case ElementType.AcikKontak:
                case ElementType.KapaliKontak:
                case ElementType.StartButon:
                case ElementType.StopButon:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 10)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 10)), this.RelSide(CPoint.Sides.Right)) };
                    break;

                case ElementType.StartStopButon:
                case ElementType.SinirAnahtar2:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 10)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 10)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 0x12)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 0x12)), this.RelSide(CPoint.Sides.Right)) };
                    break;

                case ElementType.SinirAnahtar:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 15)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 15)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(20, 7)), this.RelSide(CPoint.Sides.Right)) };
                    break;

                case ElementType.UcFazliMotor:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(20, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(40, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M) };
                    this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1], this.mCPoint[2] };
                    this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[2], this.mCPoint[0] };
                    this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[0], this.mCPoint[1] };
                    break;

                case ElementType.GucR:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(5, 30)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.R) };
                    break;

                case ElementType.GucS:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(5, 30)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.S) };
                    break;

                case ElementType.GucT:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(5, 30)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.T) };
                    break;

                case ElementType.GucN:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(5, 30)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.N) };
                    break;

                case ElementType.Startx3:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 10)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 10)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 30)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 30)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 50)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 50)), this.RelSide(CPoint.Sides.Right)) };
                    break;

                case ElementType.Start3x3:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 10)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 2)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(20, 10)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 30)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 0x16)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(20, 30)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 50)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 0x2a)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(20, 50)), this.RelSide(CPoint.Sides.Right)) };
                    break;

                case ElementType.UcFazGuc:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(5, 30)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.R), new CPoint(this, this.RelPoint(new Point(15, 30)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.S), new CPoint(this, this.RelPoint(new Point(0x19, 30)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.T) };
                    break;

                case ElementType.BirFazMotor:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 20)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(80, 20)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 80)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(80, 80)), this.RelSide(CPoint.Sides.Right)) };
                    this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                    this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                    this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[3] };
                    this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[2] };
                    break;

                case ElementType.UcFazliMotor2:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(5, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(15, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(0x19, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(5, 40)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(15, 40)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(0x19, 40)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.M) };
                    this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[4] };
                    this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[5] };
                    this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[3] };
                    this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[2] };
                    this.mCPoint[4].arrCP = new CPoint[] { this.mCPoint[0] };
                    this.mCPoint[5].arrCP = new CPoint[] { this.mCPoint[1] };
                    break;

                case ElementType.BirFazMotor2:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 20)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(80, 20)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 30)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(80, 80)), this.RelSide(CPoint.Sides.Right)) };
                    this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                    this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                    this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[3] };
                    this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[2] };
                    break;

                case ElementType.UcAcikKontak:
                case ElementType.UcKapaliKontak:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(0, 10)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 10)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 0x23)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 0x23)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(0, 60)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(20, 60)), this.RelSide(CPoint.Sides.Right)) };
                    break;

                case ElementType.Kapi1:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(130, 100)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(140, 100)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(150, 100)), this.RelSide(CPoint.Sides.Bottom), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(0, 60)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(0, 0x41)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(0, 0x55)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(0, 90)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(280, 60)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(280, 0x41)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(280, 0x55)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(280, 90)), this.RelSide(CPoint.Sides.Right)) };
                    this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1], this.mCPoint[2] };
                    this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[2], this.mCPoint[0] };
                    this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[0], this.mCPoint[1] };
                    this.mCPoint[3].arrCP = new CPoint[0];
                    this.mCPoint[4].arrCP = new CPoint[0];
                    this.mCPoint[5].arrCP = new CPoint[0];
                    this.mCPoint[6].arrCP = new CPoint[0];
                    this.mCPoint[7].arrCP = new CPoint[0];
                    this.mCPoint[8].arrCP = new CPoint[0];
                    this.mCPoint[9].arrCP = new CPoint[0];
                    this.mCPoint[10].arrCP = new CPoint[0];
                    break;

                case ElementType.Kapi2:
                    this.mCPoint = new CPoint[] { new CPoint(this, this.RelPoint(new Point(10, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(20, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(30, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(0, 70)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(0, 0x4b)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(0, 0x5f)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(0, 100)), this.RelSide(CPoint.Sides.Left)), new CPoint(this, this.RelPoint(new Point(70, 0)), this.RelSide(CPoint.Sides.Top)), new CPoint(this, this.RelPoint(new Point(0x4b, 0)), this.RelSide(CPoint.Sides.Top)), new CPoint(this, this.RelPoint(new Point(0x5f, 0)), this.RelSide(CPoint.Sides.Top)), new CPoint(this, this.RelPoint(new Point(100, 0)), this.RelSide(CPoint.Sides.Top)) };
                    this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1], this.mCPoint[2] };
                    this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[2], this.mCPoint[0] };
                    this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[0], this.mCPoint[1] };
                    this.mCPoint[3].arrCP = new CPoint[0];
                    this.mCPoint[4].arrCP = new CPoint[0];
                    this.mCPoint[5].arrCP = new CPoint[0];
                    this.mCPoint[6].arrCP = new CPoint[0];
                    this.mCPoint[7].arrCP = new CPoint[0];
                    this.mCPoint[8].arrCP = new CPoint[0];
                    this.mCPoint[9].arrCP = new CPoint[0];
                    this.mCPoint[10].arrCP = new CPoint[0];
                    break;

                case ElementType.Asansor:
                    this.mCPoint = new CPoint[] { 
                        new CPoint(this, this.RelPoint(new Point(10, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(20, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(30, 0)), this.RelSide(CPoint.Sides.Top), CPoint.Energyes.M), new CPoint(this, this.RelPoint(new Point(60, 0x2d)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 50)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 70)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0x4b)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0x55)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 90)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 110)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0x73)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0x7d)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 130)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 150)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0x9b)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0xa5)), this.RelSide(CPoint.Sides.Right)), 
                        new CPoint(this, this.RelPoint(new Point(60, 170)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 190)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0xc3)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0xcd)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 210)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 230)), this.RelSide(CPoint.Sides.Right)), new CPoint(this, this.RelPoint(new Point(60, 0xeb)), this.RelSide(CPoint.Sides.Right))
                     };
                    this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1], this.mCPoint[2] };
                    this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[2], this.mCPoint[0] };
                    this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[0], this.mCPoint[1] };
                    for (int i = 3; i < 0x17; i++)
                    {
                        this.mCPoint[i].arrCP = new CPoint[0];
                    }
                    break;

                default:
                    MessageBox.Show("HATA: Belirtilen tipler haricindi bir tip kalmiyor. -> Elements.IntCPiont()");
                    base.Size = new Size(0x15, 0x15);
                    break;
            }
            this.frmMain_Reconnect(mCPoint, this.mCPoint);
        }

        private void Initialize2()
        {
            this.mCPoint = new CPoint[0];
            this.mMovable = true;
            this.label1.Location = new Point(0, -15);
            this.r = 1.9f;
            this.mLabelLoc = new Point(0, -15);
            this.alElements = new ArrayList();
            this.label1.Tag = this;
            this.label2.Tag = this;
            this.iGroup = 0;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.label1 = new Label();
            this.timer1 = new Timer(this.components);
            this.alertTimer = new Timer(this.components);
            this.label2 = new Label();
            this.scanTimer = new Timer(this.components);
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0xb5, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(11, 11);
            this.label1.TabIndex = 0;
            this.label1.Text = "...";
            this.label1.MouseMove += new MouseEventHandler(this.label1_MouseMove);
            this.label1.MouseDown += new MouseEventHandler(this.label1_MouseDown);
            this.timer1.Interval = 0x3e8;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.alertTimer.Tick += new EventHandler(this.alertTimer_Tick);
            this.label2.AutoSize = true;
            this.label2.BackColor = Color.Transparent;
            this.label2.Enabled = false;
            this.label2.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x106, 0x11);
            this.label2.Name = "label2";
            this.label2.Size = new Size(9, 11);
            this.label2.TabIndex = 0;
            this.label2.Text = "0";
            this.label2.MouseMove += new MouseEventHandler(this.label2_MouseMove);
            this.label2.MouseDown += new MouseEventHandler(this.label2_MouseDown);
            this.scanTimer.Interval = 10;
            this.scanTimer.Tick += new EventHandler(this.scanTimer_Tick);
            base.VisibleChanged += new EventHandler(this.Elements_VisibleChanged);
            base.MouseUp += new MouseEventHandler(this.Elements_MouseUp);
            base.MouseMove += new MouseEventHandler(this.Elements_MouseMove);
            base.MouseDown += new MouseEventHandler(this.Elements_MouseDown);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            this.oPoint = new Point(e.X, e.Y);
            this.label1.Focus();
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (((e.Button == MouseButtons.Left) && this.Selected) && (this.State == 0))
            {
                this.label1.Location = new Point((this.label1.Location.X + e.X) - this.oPoint.X, (this.label1.Location.Y + e.Y) - this.oPoint.Y);
            }
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            this.oPoint = new Point(e.X, e.Y);
            this.label2.Focus();
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (((e.Button == MouseButtons.Left) && this.Selected) && (this.State == 0))
            {
                this.label2.Location = new Point((this.label2.Location.X + e.X) - this.oPoint.X, (this.label2.Location.Y + e.Y) - this.oPoint.Y);
            }
        }

        public int mCPointLength()
        {
            return this.mCPoint.Length;
        }

        private CPoint.Sides NextCPSide(CPoint.Sides s)
        {
            switch (s)
            {
                case CPoint.Sides.Left:
                    return CPoint.Sides.Top;

                case CPoint.Sides.Top:
                    return CPoint.Sides.Right;

                case CPoint.Sides.Right:
                    return CPoint.Sides.Bottom;

                case CPoint.Sides.Bottom:
                    return CPoint.Sides.Left;
            }
            return CPoint.Sides.Null;
        }

        public CPoint oCPoint(int Index)
        {
            return this.mCPoint[Index];
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.RotateTransform(this.fDegree);
            pe.Graphics.TranslateTransform((float) this.pShift.X, (float) this.pShift.Y, MatrixOrder.Append);
            switch (this.Type)
            {
                case ElementType.Role:
                    this.Paint_Role(pe);
                    break;

                case ElementType.ZamanRolesi:
                    this.Paint_ZamanRolesi(pe);
                    break;

                case ElementType.TersZamanRolesi:
                    this.Paint_TersZamanRolesi(pe);
                    break;

                case ElementType.AcikKontak:
                    this.Paint_AcikKontak(pe);
                    break;

                case ElementType.KapaliKontak:
                    this.Paint_KapaliKontak(pe);
                    break;

                case ElementType.StartButon:
                    this.Paint_StartButon(pe);
                    break;

                case ElementType.StopButon:
                    this.Paint_StopButon(pe);
                    break;

                case ElementType.StartStopButon:
                case ElementType.SinirAnahtar2:
                    this.Paint_StartStopButon(pe);
                    break;

                case ElementType.SinirAnahtar:
                    this.Paint_SinirAnahtar(pe);
                    break;

                case ElementType.Lamba:
                    this.Paint_Lamba(pe);
                    break;

                case ElementType.UcFazliMotor:
                    this.Paint_UcFazliMotor(pe);
                    break;

                case ElementType.GucR:
                    this.Paint_Guc(pe, "R");
                    break;

                case ElementType.GucS:
                    this.Paint_Guc(pe, "S");
                    break;

                case ElementType.GucT:
                    this.Paint_Guc(pe, "T");
                    break;

                case ElementType.GucN:
                    this.Paint_Guc(pe, "N");
                    break;

                case ElementType.Startx3:
                    this.Paint_Startx3(pe);
                    break;

                case ElementType.Start3x3:
                    this.Paint_Start3x3(pe);
                    break;

                case ElementType.UcFazGuc:
                    this.Paint_UcFazGuc(pe);
                    break;

                case ElementType.BirFazMotor:
                    this.Paint_BirFazMotor(pe);
                    break;

                case ElementType.UcFazliMotor2:
                    this.Paint_UcFazliMotor2(pe);
                    break;

                case ElementType.BirFazMotor2:
                    this.Paint_BirFazMotor2(pe);
                    break;

                case ElementType.UcAcikKontak:
                    this.Paint_3AcikKontak(pe);
                    break;

                case ElementType.UcKapaliKontak:
                    this.Paint_3KapaliKontak(pe);
                    break;

                case ElementType.Kapi1:
                    this.Paint_Kapi1(pe);
                    break;

                case ElementType.Kapi2:
                    this.Paint_Kapi2(pe);
                    break;

                case ElementType.Asansor:
                    this.Paint_Asansor(pe);
                    break;
            }
            pe.Graphics.RotateTransform(-this.fDegree);
            pe.Graphics.TranslateTransform((float) -this.pShift.X, (float) -this.pShift.Y, MatrixOrder.Append);
        }

        private void Paint_3AcikKontak(PaintEventArgs pe)
        {
            Pen magenta;
            float num = ((float) this.mContSize.Height) / 14f;
            float x = ((float) this.mContSize.Width) / 7f;
            float num3 = ((float) this.mContSize.Width) / 3f;
            float width = this.mContSize.Width;
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                pe.Graphics.DrawLine(magenta, new PointF(x * 6f, 0f), new PointF(x, num * 4f));
                pe.Graphics.DrawLine(magenta, new PointF(x * 6f, num * 5f), new PointF(x, num * 9f));
                pe.Graphics.DrawLine(magenta, new PointF(x * 6f, num * 10f), new PointF(x, num * 14f));
            }
            else
            {
                magenta = Pens.Black;
            }
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(num3, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(num3, 0f), new PointF(num3, num * 4f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, 0f), new PointF(num3 * 2f, num * 4f));
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 7f), new PointF(num3, num * 7f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 7f), new PointF(width, num * 7f));
            pe.Graphics.DrawLine(magenta, new PointF(num3, num * 5f), new PointF(num3, num * 9f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 5f), new PointF(num3 * 2f, num * 9f));
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 12f), new PointF(num3, num * 12f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 12f), new PointF(width, num * 12f));
            pe.Graphics.DrawLine(magenta, new PointF(num3, num * 10f), new PointF(num3, num * 14f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 10f), new PointF(num3 * 2f, num * 14f));
        }

        private void Paint_3KapaliKontak(PaintEventArgs pe)
        {
            Pen magenta;
            float num = ((float) this.mContSize.Height) / 14f;
            float x = ((float) this.mContSize.Width) / 7f;
            float num3 = ((float) this.mContSize.Width) / 3f;
            float width = this.mContSize.Width;
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
            }
            else
            {
                magenta = Pens.Black;
                pe.Graphics.DrawLine(magenta, new PointF(x * 6f, 0f), new PointF(x, num * 4f));
                pe.Graphics.DrawLine(magenta, new PointF(x * 6f, num * 5f), new PointF(x, num * 9f));
                pe.Graphics.DrawLine(magenta, new PointF(x * 6f, num * 10f), new PointF(x, num * 14f));
            }
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(num3, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(num3, 0f), new PointF(num3, num * 4f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, 0f), new PointF(num3 * 2f, num * 4f));
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 7f), new PointF(num3, num * 7f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 7f), new PointF(width, num * 7f));
            pe.Graphics.DrawLine(magenta, new PointF(num3, num * 5f), new PointF(num3, num * 9f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 5f), new PointF(num3 * 2f, num * 9f));
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 12f), new PointF(num3, num * 12f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 12f), new PointF(width, num * 12f));
            pe.Graphics.DrawLine(magenta, new PointF(num3, num * 10f), new PointF(num3, num * 14f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 10f), new PointF(num3 * 2f, num * 14f));
        }

        private void Paint_AcikKontak(PaintEventArgs pe)
        {
            Pen magenta;
            float num = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 7f;
            float single1 = ((float) this.mContSize.Height) / 3f;
            float num3 = ((float) this.mContSize.Width) / 3f;
            float height = this.mContSize.Height;
            float width = this.mContSize.Width;
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                pe.Graphics.DrawLine(magenta, new PointF(x * 6f, 0f), new PointF(x, height));
            }
            else
            {
                magenta = Pens.Black;
            }
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(num3, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(num3, 0f), new PointF(num3, num * 4f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, 0f), new PointF(num3 * 2f, num * 4f));
        }

        private void Paint_Asansor(PaintEventArgs pe)
        {
            Pen pen;
            Pen pen2;
            Pen magenta;
            Brush brush;
            Brush brush2;
            Brush brush3;
            int num7;
            int num5 = 0;
            int num6 = 0;
            float y = ((float) this.mContSize.Height) / 24f;
            float x = ((float) this.mContSize.Width) / 6f;
            float r = y;
            float num4 = 4f;
            PointF o = new PointF(x * 2f, y * 2f);
            PointF tf2 = new PointF(o.X + ((float) (Math.Cos((double) this.angle) * r)), o.Y + ((float) (Math.Sin((double) this.angle) * r)));
            Pen black = Pens.Black;
            Pen pen6 = new Pen(Color.Gray) {
                DashStyle = DashStyle.Dot
            };
            pe.Graphics.DrawRectangle(pen6, 0, 0, ((int) x) * 6, ((int) y) * 0x18);
            pe.Graphics.DrawRectangle(black, x / 2f, (float) ((int[]) this.htSysVars["asansor"])[0], (float) (((int) x) * 3), (float) (((int) y) * 3));
            pen6.Color = Color.Black;
            pen6.DashStyle = DashStyle.Dash;
            pe.Graphics.DrawLine(pen6, new PointF(x * 2f, y * 2f), new PointF(x * 2f, (float) ((int[]) this.htSysVars["asansor"])[0]));
            if (this.State == 1)
            {
                magenta = Pens.Magenta;
                brush3 = Brushes.Magenta;
                pen2 = Pens.Black;
                brush2 = Brushes.Black;
                pen = ((bool) this.htSysState["p1"]) ? Pens.Magenta : Pens.Black;
                brush = ((bool) this.htSysState["p1"]) ? Brushes.Green : Brushes.Black;
            }
            else
            {
                magenta = Pens.Black;
                pen2 = Pens.Black;
                pen = Pens.Black;
                brush3 = Brushes.Black;
                brush2 = Brushes.Black;
                brush = Brushes.Black;
                this.htSysStateSet("p6", true);
            }
            pe.Graphics.DrawEllipse(pen, this.sqrRectF(o, r));
            pe.Graphics.DrawLine(pen, new PointF(x, y * 2f), new PointF(x, 0f));
            pe.Graphics.DrawLine(pen, new PointF(x * 2f, y), new PointF(x * 2f, 0f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, y * 2f), new PointF(x * 3f, 0f));
            if (this.State != 0)
            {
                pe.Graphics.FillEllipse(brush, this.sqrRectF(tf2, num4));
            }
            for (num7 = 0; num7 < 5; num7++)
            {
                Pen pen4 = ((bool) this.htSysState["p" + (num7 + 2)]) ? magenta : pen2;
                Brush brush4 = ((bool) this.htSysState["p" + (num7 + 2)]) ? brush3 : brush2;
                num5 = ((bool) this.htSysState["p" + (num7 + 2)]) ? 5 : 0;
                num6 = (((int) y) * 4) * (num7 + 1);
                pe.Graphics.FillEllipse(brush4, this.sqrRectF(new PointF(x * 5f, (num6 + y) + (y / 2f)), this.r));
                pe.Graphics.FillEllipse(brush4, this.sqrRectF(new PointF(x * 5f, (num6 + (y * 2f)) + (y / 2f)), this.r));
                pe.Graphics.FillEllipse(brush4, this.sqrRectF(new PointF((x * 5f) + (x / 2f), (num6 + y) + (y / 2f)), this.r));
                pe.Graphics.FillEllipse(brush4, this.sqrRectF(new PointF((x * 5f) + (x / 2f), (num6 + (y * 2f)) + (y / 2f)), this.r));
                PointF[] tfArray2 = new PointF[] { new PointF(x * 5f, (num6 + y) + (y / 2f)), new PointF(x * 5f, num6 + (y / 2f)), new PointF(x * 6f, num6 + (y / 2f)) };
                PointF[] points = tfArray2;
                pe.Graphics.DrawLines(pen4, points);
                tfArray2 = new PointF[] { new PointF(x * 5f, (num6 + (y * 2f)) + (y / 2f)), new PointF(x * 5f, (num6 + (y * 3f)) + (y / 2f)), new PointF(x * 6f, (num6 + (y * 3f)) + (y / 2f)) };
                points = tfArray2;
                pe.Graphics.DrawLines(pen4, points);
                tfArray2 = new PointF[] { new PointF((x * 5f) + (x / 2f), (num6 + y) + (y / 2f)), new PointF((x * 5f) + (x / 2f), num6 + y), new PointF(x * 6f, num6 + y) };
                points = tfArray2;
                pe.Graphics.DrawLines(pen4, points);
                tfArray2 = new PointF[] { new PointF((x * 5f) + (x / 2f), (num6 + (y * 2f)) + (y / 2f)), new PointF((x * 5f) + (x / 2f), num6 + (y * 3f)), new PointF(x * 6f, num6 + (y * 3f)) };
                points = tfArray2;
                pe.Graphics.DrawLines(pen4, points);
                pe.Graphics.DrawLine(pen4, new PointF((x * 5f) + num5, (num6 + y) + (y / 2f)), new PointF((x * 5f) + num5, (num6 + (y * 2f)) + (y / 2f)));
                pe.Graphics.DrawLine(pen4, new PointF(((x * 3f) + (x / 2f)) + num5, num6 + (y * 2f)), new PointF((x * 5f) + num5, num6 + (y * 2f)));
            }
            pe.Graphics.DrawRectangle(black, (float) 0f, (float) (y * 4f), (float) ((x * 4f) + (x / 2f)), (float) (y * 20f));
            for (num7 = 0; num7 < 4; num7++)
            {
                pe.Graphics.DrawLine(black, new PointF(0f, (y * 4f) * (num7 + 2)), new PointF((x * 4f) + (x / 2f), (y * 4f) * (num7 + 2)));
            }
        }

        private void Paint_BirFazMotor(PaintEventArgs pe)
        {
            Pen magenta;
            Pen pen2;
            Pen pen3;
            Brush green;
            float tension = 0.7f;
            float num = ((float) this.mContSize.Height) / 10f;
            float x = ((float) this.mContSize.Width) / 8f;
            float r = (num * 2f) - 1f;
            float num5 = 4f;
            float num6 = num / 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            PointF o = new PointF(x * 5f, num * 5f);
            PointF tf2 = new PointF(o.X + ((float) (Math.Cos((double) this.angle) * r)), o.Y + ((float) (Math.Sin((double) this.angle) * r)));
            Brush black = Brushes.Black;
            switch (this.State)
            {
                case 1:
                    magenta = Pens.Black;
                    pen2 = Pens.Black;
                    pen3 = Pens.Black;
                    green = Brushes.Black;
                    break;

                case 2:
                    magenta = Pens.Magenta;
                    pen2 = Pens.Black;
                    pen3 = Pens.Black;
                    green = Brushes.Black;
                    break;

                case 3:
                    magenta = Pens.Magenta;
                    pen2 = Pens.Black;
                    pen3 = Pens.Magenta;
                    green = Brushes.Green;
                    break;

                case 4:
                    magenta = Pens.Black;
                    pen2 = Pens.Magenta;
                    pen3 = Pens.Black;
                    green = Brushes.Black;
                    break;

                case 5:
                    magenta = Pens.Magenta;
                    pen2 = Pens.Magenta;
                    pen3 = Pens.Magenta;
                    green = Brushes.Green;
                    break;

                default:
                    magenta = Pens.Black;
                    pen2 = Pens.Black;
                    pen3 = Pens.Black;
                    green = Brushes.Black;
                    break;
            }
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(x * 3f, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(x * 7f, num * 2f), new PointF(width, num * 2f));
            float num7 = x * 3f;
            float y = num * 2f;
            PointF[] tfArray2 = new PointF[] { 
                new PointF(num7, y), new PointF(num7 + num6, y - num6), new PointF(num7 + (num6 * 2f), y + num6), new PointF(num7 + num6, y + num6), new PointF(num7 + (num6 * 3f), y - num6), new PointF(num7 + (num6 * 4f), y + num6), new PointF(num7 + (num6 * 3f), y + num6), new PointF(num7 + (num6 * 5f), y - num6), new PointF(num7 + (num6 * 6f), y + num6), new PointF(num7 + (num6 * 5f), y + num6), new PointF(num7 + (num6 * 7f), y - num6), new PointF(num7 + (num6 * 8f), y + num6), new PointF(num7 + (num6 * 7f), y + num6), new PointF(num7 + (num6 * 9f), y - num6), new PointF(num7 + (num6 * 10f), y + num6), new PointF(num7 + (num6 * 9f), y + num6), 
                new PointF(num7 + (num6 * 11f), y - num6), new PointF(num7 + (num6 * 12f), y + num6), new PointF(num7 + (num6 * 11f), y + num6), new PointF(num7 + (num6 * 13f), y - num6), new PointF(num7 + (num6 * 14f), y + num6), new PointF(num7 + (num6 * 13f), y + num6), new PointF(num7 + (num6 * 15f), y - num6), new PointF(num7 + (num6 * 16f), y + num6), new PointF(num7 + (num6 * 15f), y + num6), new PointF(num7 + (num6 * 16f), y)
             };
            PointF[] points = tfArray2;
            pe.Graphics.DrawCurve(magenta, points, tension);
            pe.Graphics.DrawEllipse(pen3, this.sqrRectF(o, r));
            pe.Graphics.DrawEllipse(pen3, this.sqrRectF(o, num / 2f));
            for (int i = 0; i < 7; i++)
            {
                PointF tf3 = new PointF(o.X + ((float) (((Math.Cos((6.2831853071795862 * i) / 7.0) * r) * 2.0) / 3.0)), o.Y + ((float) (((Math.Sin((6.2831853071795862 * i) / 7.0) * r) * 2.0) / 3.0)));
                pe.Graphics.DrawEllipse(pen3, this.sqrRectF(tf3, num5));
            }
            if (this.State != 0)
            {
                pe.Graphics.FillEllipse(green, this.sqrRectF(tf2, num5));
            }
            pe.Graphics.DrawLine(pen2, new PointF(0f, num * 8f), new PointF(x, num * 8f));
            pe.Graphics.DrawLine(pen2, new PointF(x + (num6 * 2f), num * 8f), new PointF(x * 3f, num * 8f));
            pe.Graphics.DrawLine(pen2, new PointF(x, (num * 8f) - (num6 * 2f)), new PointF(x, (num * 8f) + (num6 * 2f)));
            pe.Graphics.DrawLine(pen2, new PointF(x * 7f, num * 8f), new PointF(width, num * 8f));
            num7 = x * 3f;
            y = num * 8f;
            tfArray2 = new PointF[] { 
                new PointF(num7, y), new PointF(num7 + num6, y - num6), new PointF(num7 + (num6 * 2f), y + num6), new PointF(num7 + num6, y + num6), new PointF(num7 + (num6 * 3f), y - num6), new PointF(num7 + (num6 * 4f), y + num6), new PointF(num7 + (num6 * 3f), y + num6), new PointF(num7 + (num6 * 5f), y - num6), new PointF(num7 + (num6 * 6f), y + num6), new PointF(num7 + (num6 * 5f), y + num6), new PointF(num7 + (num6 * 7f), y - num6), new PointF(num7 + (num6 * 8f), y + num6), new PointF(num7 + (num6 * 7f), y + num6), new PointF(num7 + (num6 * 9f), y - num6), new PointF(num7 + (num6 * 10f), y + num6), new PointF(num7 + (num6 * 9f), y + num6), 
                new PointF(num7 + (num6 * 11f), y - num6), new PointF(num7 + (num6 * 12f), y + num6), new PointF(num7 + (num6 * 11f), y + num6), new PointF(num7 + (num6 * 13f), y - num6), new PointF(num7 + (num6 * 14f), y + num6), new PointF(num7 + (num6 * 13f), y + num6), new PointF(num7 + (num6 * 15f), y - num6), new PointF(num7 + (num6 * 16f), y + num6), new PointF(num7 + (num6 * 15f), y + num6), new PointF(num7 + (num6 * 16f), y)
             };
            points = tfArray2;
            pe.Graphics.DrawCurve(pen2, points, tension);
            tfArray2 = new PointF[] { new PointF(x + (num6 * 3f), (num * 8f) - (num6 * 2f)), new PointF(x + (num6 * 2f), num * 8f), new PointF(x + (num6 * 3f), (num * 8f) + (num6 * 2f)) };
            points = tfArray2;
            tension = 2f;
            pe.Graphics.DrawCurve(pen2, points, tension);
            pe.Graphics.DrawString("A.S.", this.Font, black, (float) (x * 4f), (float) 0f);
            pe.Graphics.DrawString("Y.S.", this.Font, black, (float) (x * 4f), (float) (num * 9f));
            pe.Graphics.DrawString("C1", this.Font, black, x, num * 9f);
        }

        private void Paint_BirFazMotor2(PaintEventArgs pe)
        {
            float num9 = 0f;
            float tension = 0.7f;
            float num = ((float) this.mContSize.Height) / 10f;
            float x = ((float) this.mContSize.Width) / 8f;
            float r = (num * 2f) - 1f;
            float num5 = 4f;
            float num6 = num / 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            PointF o = new PointF(x * 5f, num * 5f);
            PointF tf2 = new PointF(o.X + ((float) (Math.Cos((double) this.angle) * r)), o.Y + ((float) (Math.Sin((double) this.angle) * r)));
            Pen black = Pens.Black;
            Pen pen = Pens.Black;
            Pen pen4 = new Pen(Color.Black);
            Pen magenta = Pens.Black;
            Brush green = Brushes.Black;
            Brush brush2 = Brushes.Black;
            Brush brush3 = Brushes.Black;
            switch (this.State)
            {
                case 2:
                    black = Pens.Magenta;
                    break;

                case 3:
                    black = Pens.Magenta;
                    magenta = Pens.Magenta;
                    green = Brushes.Green;
                    num9 = 4f;
                    break;

                case 4:
                    pen = Pens.Magenta;
                    brush3 = Brushes.Magenta;
                    break;

                case 5:
                    black = Pens.Magenta;
                    pen = Pens.Magenta;
                    pen4 = new Pen(Color.Magenta);
                    magenta = Pens.Magenta;
                    green = Brushes.Green;
                    brush3 = Brushes.Magenta;
                    break;
            }
            pen4.DashStyle = DashStyle.Dash;
            pe.Graphics.DrawLine(black, new PointF(0f, num * 2f), new PointF(x * 3f, num * 2f));
            pe.Graphics.DrawLine(black, new PointF(x * 7f, num * 2f), new PointF(width, num * 2f));
            float num7 = x * 3f;
            float y = num * 2f;
            PointF[] tfArray2 = new PointF[] { 
                new PointF(num7, y), new PointF(num7 + num6, y - num6), new PointF(num7 + (num6 * 2f), y + num6), new PointF(num7 + num6, y + num6), new PointF(num7 + (num6 * 3f), y - num6), new PointF(num7 + (num6 * 4f), y + num6), new PointF(num7 + (num6 * 3f), y + num6), new PointF(num7 + (num6 * 5f), y - num6), new PointF(num7 + (num6 * 6f), y + num6), new PointF(num7 + (num6 * 5f), y + num6), new PointF(num7 + (num6 * 7f), y - num6), new PointF(num7 + (num6 * 8f), y + num6), new PointF(num7 + (num6 * 7f), y + num6), new PointF(num7 + (num6 * 9f), y - num6), new PointF(num7 + (num6 * 10f), y + num6), new PointF(num7 + (num6 * 9f), y + num6), 
                new PointF(num7 + (num6 * 11f), y - num6), new PointF(num7 + (num6 * 12f), y + num6), new PointF(num7 + (num6 * 11f), y + num6), new PointF(num7 + (num6 * 13f), y - num6), new PointF(num7 + (num6 * 14f), y + num6), new PointF(num7 + (num6 * 13f), y + num6), new PointF(num7 + (num6 * 15f), y - num6), new PointF(num7 + (num6 * 16f), y + num6), new PointF(num7 + (num6 * 15f), y + num6), new PointF(num7 + (num6 * 16f), y)
             };
            PointF[] points = tfArray2;
            pe.Graphics.DrawCurve(black, points, tension);
            pe.Graphics.DrawEllipse(magenta, this.sqrRectF(o, r));
            pe.Graphics.DrawEllipse(magenta, this.sqrRectF(o, num / 2f));
            for (int i = 0; i < 7; i++)
            {
                PointF tf3 = new PointF(o.X + ((float) (((Math.Cos((6.2831853071795862 * i) / 7.0) * r) * 2.0) / 3.0)), o.Y + ((float) (((Math.Sin((6.2831853071795862 * i) / 7.0) * r) * 2.0) / 3.0)));
                pe.Graphics.DrawEllipse(magenta, this.sqrRectF(tf3, num5));
            }
            if (this.State != 0)
            {
                pe.Graphics.FillEllipse(green, this.sqrRectF(tf2, num5));
            }
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 3f), new PointF(x / 2f, num * 3f));
            pe.Graphics.DrawLine(pen, new PointF(x / 2f, num * 3f), new PointF(x / 2f, (num * 9f) / 2f));
            pe.Graphics.DrawLine(pen, new PointF(x / 2f, (num * 11f) / 2f), new PointF(x / 2f, num * 8f));
            pe.Graphics.DrawLine(pen, new PointF((x / 2f) + num9, (num * 9f) / 2f), new PointF(x / 2f, (num * 11f) / 2f));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF(x / 2f, (num * 11f) / 2f), this.r));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF(x / 2f, (num * 9f) / 2f), this.r));
            pe.Graphics.DrawLine(pen4, new PointF(x, num * 5f), new PointF((x * 5f) / 2f, num * 5f));
            pe.Graphics.DrawLine(pen, new PointF(x / 2f, num * 8f), new PointF(x, num * 8f));
            pe.Graphics.DrawLine(pen, new PointF(x / 2f, num * 8f), new PointF(x, num * 8f));
            pe.Graphics.DrawLine(pen, new PointF(x + (num6 * 2f), num * 8f), new PointF(x * 3f, num * 8f));
            pe.Graphics.DrawLine(pen, new PointF(x, (num * 8f) - (num6 * 2f)), new PointF(x, (num * 8f) + (num6 * 2f)));
            pe.Graphics.DrawLine(pen, new PointF(x * 7f, num * 8f), new PointF(x * 8f, num * 8f));
            num7 = x * 3f;
            y = num * 8f;
            tfArray2 = new PointF[] { 
                new PointF(num7, y), new PointF(num7 + num6, y - num6), new PointF(num7 + (num6 * 2f), y + num6), new PointF(num7 + num6, y + num6), new PointF(num7 + (num6 * 3f), y - num6), new PointF(num7 + (num6 * 4f), y + num6), new PointF(num7 + (num6 * 3f), y + num6), new PointF(num7 + (num6 * 5f), y - num6), new PointF(num7 + (num6 * 6f), y + num6), new PointF(num7 + (num6 * 5f), y + num6), new PointF(num7 + (num6 * 7f), y - num6), new PointF(num7 + (num6 * 8f), y + num6), new PointF(num7 + (num6 * 7f), y + num6), new PointF(num7 + (num6 * 9f), y - num6), new PointF(num7 + (num6 * 10f), y + num6), new PointF(num7 + (num6 * 9f), y + num6), 
                new PointF(num7 + (num6 * 11f), y - num6), new PointF(num7 + (num6 * 12f), y + num6), new PointF(num7 + (num6 * 11f), y + num6), new PointF(num7 + (num6 * 13f), y - num6), new PointF(num7 + (num6 * 14f), y + num6), new PointF(num7 + (num6 * 13f), y + num6), new PointF(num7 + (num6 * 15f), y - num6), new PointF(num7 + (num6 * 16f), y + num6), new PointF(num7 + (num6 * 15f), y + num6), new PointF(num7 + (num6 * 16f), y)
             };
            points = tfArray2;
            pe.Graphics.DrawCurve(pen, points, tension);
            tfArray2 = new PointF[] { new PointF(x + (num6 * 3f), (num * 8f) - (num6 * 2f)), new PointF(x + (num6 * 2f), num * 8f), new PointF(x + (num6 * 3f), (num * 8f) + (num6 * 2f)) };
            points = tfArray2;
            tension = 2f;
            pe.Graphics.DrawCurve(pen, points, tension);
            pe.Graphics.DrawString("A.S.", this.Font, brush2, (float) (x * 4f), (float) 0f);
            pe.Graphics.DrawString("Y.S.", this.Font, brush2, (float) (x * 4f), (float) (num * 9f));
            pe.Graphics.DrawString("C1", this.Font, brush2, x, num * 9f);
        }

        private void Paint_Guc(PaintEventArgs pe, string s)
        {
            Pen black = Pens.Black;
            Brush brush = Brushes.Black;
            Rectangle rectangle = new Rectangle(0, 0, 10, this.mContSize.Height);
            float num2 = ((float) this.mContSize.Height) / 3f;
            float single1 = ((float) this.mContSize.Height) / 2f;
            float x = ((float) this.mContSize.Width) / 2f;
            float r = x / 2f;
            float height = rectangle.Height;
            pe.Graphics.DrawString(s, this.Font, brush, (float) 0f, (float) 0f);
            PointF o = new PointF(x, num2 * 2f);
            pe.Graphics.DrawEllipse(black, this.sqrRect(o, r));
            pe.Graphics.DrawLine(black, new PointF(x, (num2 * 2f) + r), new PointF(x, height));
        }

        private void Paint_KapaliKontak(PaintEventArgs pe)
        {
            Pen magenta;
            float num = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 7f;
            float single1 = ((float) this.mContSize.Height) / 3f;
            float num3 = ((float) this.mContSize.Width) / 3f;
            float height = this.mContSize.Height;
            float width = this.mContSize.Width;
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
            }
            else
            {
                magenta = Pens.Black;
                pe.Graphics.DrawLine(magenta, new PointF(x * 6f, 0f), new PointF(x, height));
            }
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(num3, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(num3, 0f), new PointF(num3, height));
            pe.Graphics.DrawLine(magenta, new PointF(num3 * 2f, 0f), new PointF(num3 * 2f, height));
        }

        private void Paint_Kapi1(PaintEventArgs pe)
        {
            Pen pen;
            Pen pen2;
            Pen pen3;
            Brush brush;
            Brush brush2;
            Brush brush3;
            int num5 = 0;
            int num6 = 0;
            float num = ((float) this.mContSize.Height) / 10f;
            float x = ((float) this.mContSize.Width) / 28f;
            float r = num;
            float num4 = 4f;
            PointF o = new PointF(x * 14f, num * 8f);
            PointF tf2 = new PointF(o.X + ((float) (Math.Cos((double) this.angle) * r)), o.Y + ((float) (Math.Sin((double) this.angle) * r)));
            Pen black = Pens.Black;
            Pen pen5 = new Pen(Color.Gray) {
                DashStyle = DashStyle.Dot
            };
            pe.Graphics.DrawRectangle(pen5, 0, 0, ((int) x) * 0x1c, ((int) num) * 10);
            pe.Graphics.DrawRectangle(black, ((int[]) this.htSysVars["kapi"])[0], 0, ((int) x) * 13, ((int) num) * 9);
            if (this.State == 1)
            {
                pen3 = ((bool) this.htSysState["p3"]) ? Pens.Magenta : Pens.Black;
                brush3 = ((bool) this.htSysState["p3"]) ? Brushes.Magenta : Brushes.Black;
                pen2 = ((bool) this.htSysState["p2"]) ? Pens.Magenta : Pens.Black;
                brush2 = ((bool) this.htSysState["p2"]) ? Brushes.Magenta : Brushes.Black;
                pen = ((bool) this.htSysState["p1"]) ? Pens.Magenta : Pens.Black;
                brush = ((bool) this.htSysState["p1"]) ? Brushes.Green : Brushes.Black;
                num6 = ((bool) this.htSysState["p3"]) ? 5 : 0;
                num5 = ((bool) this.htSysState["p2"]) ? -5 : 0;
            }
            else
            {
                pen3 = Pens.Black;
                pen2 = Pens.Black;
                pen = Pens.Black;
                brush3 = Brushes.Black;
                brush2 = Brushes.Black;
                brush = Brushes.Black;
                num5 = -5;
            }
            pe.Graphics.DrawEllipse(pen, this.sqrRectF(o, r));
            pe.Graphics.DrawLine(pen, new PointF(x * 13f, num * 8f), new PointF(x * 13f, num * 10f));
            pe.Graphics.DrawLine(pen, new PointF(x * 14f, num * 9f), new PointF(x * 14f, num * 10f));
            pe.Graphics.DrawLine(pen, new PointF(x * 15f, num * 8f), new PointF(x * 15f, num * 10f));
            if (this.State != 0)
            {
                pe.Graphics.FillEllipse(brush, this.sqrRectF(tf2, num4));
            }
            pe.Graphics.FillEllipse(brush2, this.sqrRectF(new PointF(x, num * 7f), this.r));
            pe.Graphics.FillEllipse(brush2, this.sqrRectF(new PointF(x, num * 8f), this.r));
            pe.Graphics.FillEllipse(brush2, this.sqrRectF(new PointF(x + (x / 2f), num * 7f), this.r));
            pe.Graphics.FillEllipse(brush2, this.sqrRectF(new PointF(x + (x / 2f), num * 8f), this.r));
            PointF[] tfArray2 = new PointF[] { new PointF(0f, (num * 6f) + (num / 2f)), new PointF(x, (num * 6f) + (num / 2f)), new PointF(x, num * 7f) };
            PointF[] points = tfArray2;
            pe.Graphics.DrawLines(pen2, points);
            tfArray2 = new PointF[] { new PointF(0f, (num * 8f) + (num / 2f)), new PointF(x, (num * 8f) + (num / 2f)), new PointF(x, num * 8f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen2, points);
            tfArray2 = new PointF[] { new PointF(0f, num * 6f), new PointF(x + (x / 2f), num * 6f), new PointF(x + (x / 2f), num * 7f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen2, points);
            tfArray2 = new PointF[] { new PointF(0f, num * 9f), new PointF(x + (x / 2f), num * 9f), new PointF(x + (x / 2f), num * 8f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen2, points);
            pe.Graphics.DrawLine(pen2, new PointF((x + (x / 2f)) + num5, num * 7f), new PointF((x + (x / 2f)) + num5, num * 8f));
            pe.Graphics.DrawLine(pen2, new PointF((x + (x / 2f)) + num5, (num * 7f) + (num / 2f)), new PointF(((x * 2f) + (x / 2f)) + num5, (num * 7f) + (num / 2f)));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF(x * 27f, num * 7f), this.r));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF(x * 27f, num * 8f), this.r));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF((x * 26f) + (x / 2f), num * 7f), this.r));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF((x * 26f) + (x / 2f), num * 8f), this.r));
            tfArray2 = new PointF[] { new PointF(x * 28f, (num * 6f) + (num / 2f)), new PointF(x * 27f, (num * 6f) + (num / 2f)), new PointF(x * 27f, num * 7f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen3, points);
            tfArray2 = new PointF[] { new PointF(x * 28f, (num * 8f) + (num / 2f)), new PointF(x * 27f, (num * 8f) + (num / 2f)), new PointF(x * 27f, num * 8f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen3, points);
            tfArray2 = new PointF[] { new PointF(x * 28f, num * 6f), new PointF((x * 26f) + (x / 2f), num * 6f), new PointF((x * 26f) + (x / 2f), num * 7f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen3, points);
            tfArray2 = new PointF[] { new PointF(x * 28f, num * 9f), new PointF((x * 26f) + (x / 2f), num * 9f), new PointF((x * 26f) + (x / 2f), num * 8f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen3, points);
            pe.Graphics.DrawLine(pen3, new PointF(((x * 26f) + (x / 2f)) + num6, num * 7f), new PointF(((x * 26f) + (x / 2f)) + num6, num * 8f));
            pe.Graphics.DrawLine(pen3, new PointF(((x * 26f) + (x / 2f)) + num6, (num * 7f) + (num / 2f)), new PointF(((x * 25f) + (x / 2f)) + num6, (num * 7f) + (num / 2f)));
        }

        private void Paint_Kapi2(PaintEventArgs pe)
        {
            Pen pen;
            Pen pen2;
            Pen pen3;
            Brush brush;
            Brush brush2;
            Brush brush3;
            int num7 = 0;
            int num8 = 0;
            float y = ((float) this.mContSize.Height) / 10f;
            float x = ((float) this.mContSize.Width) / 10f;
            float r = y;
            float num4 = 4f;
            PointF o = new PointF(x * 2f, y * 2f);
            PointF tf2 = new PointF(o.X + ((float) (Math.Cos((double) -this.angle) * r)), o.Y + ((float) (Math.Sin((double) -this.angle) * r)));
            Pen black = Pens.Black;
            Pen pen5 = new Pen(Color.Gray) {
                DashStyle = DashStyle.Dot
            };
            Brush white = Brushes.White;
            pe.Graphics.DrawRectangle(pen5, 0, 0, ((int) x) * 10, ((int) y) * 10);
            float dx = (float) (20.0 - ((20.0 * Math.Sqrt(2.0)) * Math.Cos(((0x2d - ((int[]) this.htSysVars["kapi"])[0]) * 3.1415926535897931) / 180.0)));
            float dy = (float) (20.0 - ((20.0 * Math.Sqrt(2.0)) * Math.Sin(((0x2d - ((int[]) this.htSysVars["kapi"])[0]) * 3.1415926535897931) / 180.0)));
            pe.Graphics.RotateTransform((float) -((int[]) this.htSysVars["kapi"])[0]);
            pe.Graphics.TranslateTransform(dx, dy, MatrixOrder.Append);
            pe.Graphics.DrawRectangle(black, (int) (x + (x / 2f)), ((int) y) * 2, (int) x, ((int) y) * 8);
            pe.Graphics.RotateTransform((float) ((int[]) this.htSysVars["kapi"])[0]);
            pe.Graphics.TranslateTransform(-dx, -dy, MatrixOrder.Append);
            if (this.State == 1)
            {
                pen3 = ((bool) this.htSysState["p3"]) ? Pens.Magenta : Pens.Black;
                brush3 = ((bool) this.htSysState["p3"]) ? Brushes.Magenta : Brushes.Black;
                pen2 = ((bool) this.htSysState["p2"]) ? Pens.Magenta : Pens.Black;
                brush2 = ((bool) this.htSysState["p2"]) ? Brushes.Magenta : Brushes.Black;
                pen = ((bool) this.htSysState["p1"]) ? Pens.Magenta : Pens.Black;
                brush = ((bool) this.htSysState["p1"]) ? Brushes.Green : Brushes.Black;
                num8 = ((bool) this.htSysState["p3"]) ? -5 : 0;
                num7 = ((bool) this.htSysState["p2"]) ? -5 : 0;
            }
            else
            {
                pen3 = Pens.Black;
                pen2 = Pens.Black;
                pen = Pens.Black;
                brush3 = Brushes.Black;
                brush2 = Brushes.Black;
                brush = Brushes.Black;
                num7 = -5;
            }
            pe.Graphics.FillEllipse(white, this.sqrRectF(o, r));
            pe.Graphics.DrawEllipse(pen, this.sqrRectF(o, r));
            pe.Graphics.DrawLine(pen, new PointF(x, y * 2f), new PointF(x, 0f));
            pe.Graphics.DrawLine(pen, new PointF(x * 2f, y), new PointF(x * 2f, 0f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, y * 2f), new PointF(x * 3f, 0f));
            if (this.State != 0)
            {
                pe.Graphics.FillEllipse(brush, this.sqrRectF(tf2, num4));
            }
            pe.Graphics.FillEllipse(brush2, this.sqrRectF(new PointF(x / 2f, y * 8f), this.r));
            pe.Graphics.FillEllipse(brush2, this.sqrRectF(new PointF(x / 2f, y * 9f), this.r));
            pe.Graphics.FillEllipse(brush2, this.sqrRectF(new PointF(x, y * 8f), this.r));
            pe.Graphics.FillEllipse(brush2, this.sqrRectF(new PointF(x, y * 9f), this.r));
            PointF[] tfArray2 = new PointF[] { new PointF(0f, (y * 7f) + (y / 2f)), new PointF(x / 2f, (y * 7f) + (y / 2f)), new PointF(x / 2f, y * 8f) };
            PointF[] points = tfArray2;
            pe.Graphics.DrawLines(pen2, points);
            tfArray2 = new PointF[] { new PointF(0f, (y * 9f) + (y / 2f)), new PointF(x / 2f, (y * 9f) + (y / 2f)), new PointF(x / 2f, y * 9f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen2, points);
            tfArray2 = new PointF[] { new PointF(0f, y * 7f), new PointF(x, y * 7f), new PointF(x, y * 8f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen2, points);
            tfArray2 = new PointF[] { new PointF(0f, y * 10f), new PointF(x, y * 10f), new PointF(x, y * 9f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen2, points);
            pe.Graphics.DrawLine(pen2, new PointF(x + num7, y * 8f), new PointF(x + num7, y * 9f));
            pe.Graphics.DrawLine(pen2, new PointF(x + num7, (y * 8f) + (y / 2f)), new PointF((x * 2f) + num7, (y * 8f) + (y / 2f)));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF(x * 8f, y / 2f), this.r));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF(x * 9f, y / 2f), this.r));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF(x * 8f, y), this.r));
            pe.Graphics.FillEllipse(brush3, this.sqrRectF(new PointF(x * 9f, y), this.r));
            tfArray2 = new PointF[] { new PointF(x * 7f, 0f), new PointF(x * 7f, y), new PointF(x * 8f, y) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen3, points);
            tfArray2 = new PointF[] { new PointF(x * 10f, 0f), new PointF(x * 10f, y), new PointF(x * 9f, y) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen3, points);
            tfArray2 = new PointF[] { new PointF((x * 7f) + (x / 2f), 0f), new PointF((x * 7f) + (x / 2f), y / 2f), new PointF(x * 8f, y / 2f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen3, points);
            tfArray2 = new PointF[] { new PointF((x * 9f) + (x / 2f), 0f), new PointF((x * 9f) + (x / 2f), y / 2f), new PointF(x * 9f, y / 2f) };
            points = tfArray2;
            pe.Graphics.DrawLines(pen3, points);
            pe.Graphics.DrawLine(pen3, new PointF(x * 8f, y + num8), new PointF(x * 9f, y + num8));
            pe.Graphics.DrawLine(pen3, new PointF((x * 8f) + (x / 2f), y + num8), new PointF((x * 8f) + (x / 2f), (y * 2f) + num8));
        }

        private void Paint_Lamba(PaintEventArgs pe)
        {
            Pen magenta;
            Brush black;
            float num = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            float r = x;
            PointF o = new PointF(x * 2f, num * 2f);
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                black = new SolidBrush(this.mColor);
                pe.Graphics.FillEllipse(black, this.sqrRectF(o, r));
            }
            else
            {
                magenta = Pens.Black;
                black = Brushes.Black;
                pe.Graphics.DrawEllipse(magenta, this.sqrRectF(o, r));
            }
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(x * 3f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF((((float) Math.Sin(0.78539816339744828)) * r) + o.X, (((float) Math.Cos(0.78539816339744828)) * r) + o.Y), new PointF((((float) Math.Sin(3.9269908169872414)) * r) + o.X, (((float) Math.Cos(3.9269908169872414)) * r) + o.Y));
            pe.Graphics.DrawLine(magenta, new PointF((((float) Math.Sin(2.3561944901923448)) * r) + o.X, (((float) Math.Cos(2.3561944901923448)) * r) + o.Y), new PointF((((float) Math.Sin(5.497787143782138)) * r) + o.X, (((float) Math.Cos(5.497787143782138)) * r) + o.Y));
        }

        private void Paint_Role(PaintEventArgs pe)
        {
            Pen pen;
            if (this.State == 2)
            {
                pen = new Pen(Color.Magenta);
            }
            else
            {
                pen = new Pen(Color.Black);
            }
            float num = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 4f;
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, num * 2f), new PointF(x * 4f, num * 2f));
            pe.Graphics.DrawRectangle(pen, x, 0f, x * 2f, num * 4f);
            pe.Graphics.DrawLine(pen, x * 3f, num, x, num * 3f);
        }

        private void Paint_SinirAnahtar(PaintEventArgs pe)
        {
            float num4;
            Pen magenta;
            Brush black;
            int num8 = 4;
            int num9 = 9;
            float num = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            float y = ((num * 3f) - (2f * this.r)) - num8;
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                black = Brushes.Magenta;
                num4 = num * 3f;
            }
            else
            {
                magenta = Pens.Black;
                black = Brushes.Black;
                num4 = ((num * 3f) - this.r) - num8;
            }
            float num5 = (num * 3f) - (((num * 3f) - num4) / 2f);
            float num6 = num5 - num9;
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 3f), new PointF(x, num * 3f));
            pe.Graphics.DrawLine(magenta, new PointF(x * 3f, num * 3f), new PointF(width, num * 3f));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x, num * 3f), this.r));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x * 3f, num * 3f), this.r));
            pe.Graphics.DrawLine(magenta, new PointF(x * 3f, y), new PointF(width, y));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x * 3f, y), this.r));
            pe.Graphics.DrawLine(magenta, new PointF(x, num * 3f), new PointF(x * 3f, num4));
            pe.Graphics.DrawLine(magenta, new PointF(x * 2f, num6), new PointF(x * 2f, num5));
            pe.Graphics.DrawEllipse(magenta, this.sqrRect(new PointF(x * 2f, num6), num / 2f));
        }

        private void Paint_Start3x3(PaintEventArgs pe)
        {
            float num4;
            Brush magenta;
            Pen pen = new Pen(Color.Black);
            float num = ((float) this.mContSize.Height) / 12f;
            float x = ((float) this.mContSize.Width) / 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            int num6 = 7;
            int num5 = 4;
            if (this.State == 2)
            {
                pen.Color = Color.Magenta;
                magenta = Brushes.Magenta;
                num4 = 0f;
            }
            else
            {
                pen.Color = Color.Black;
                magenta = Brushes.Black;
                num4 = this.r + num5;
            }
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 6f), new PointF(x, num * 6f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, num * 6f), new PointF(width, num * 6f));
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 10f), new PointF(x, num * 10f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, num * 10f), new PointF(width, num * 10f));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x, num * 2f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, num * 2f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x, num * 6f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, num * 6f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x, num * 10f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, num * 10f), this.r));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, ((num * 2f) - (this.r * 2f)) - num5), new PointF(width, ((num * 2f) - (this.r * 2f)) - num5));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, ((num * 6f) - (this.r * 2f)) - num5), new PointF(width, ((num * 6f) - (this.r * 2f)) - num5));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, ((num * 10f) - (this.r * 2f)) - num5), new PointF(width, ((num * 10f) - (this.r * 2f)) - num5));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, ((num * 2f) - (this.r * 2f)) - num5), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, ((num * 6f) - (this.r * 2f)) - num5), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, ((num * 10f) - (this.r * 2f)) - num5), this.r));
            pe.Graphics.DrawLine(pen, new PointF(x, num * 2f), new PointF(x * 3f, (num * 2f) - num4));
            pe.Graphics.DrawLine(pen, new PointF(x * 2f, ((num * 2f) - (num4 / 2f)) - num6), new PointF(x * 2f, (num * 2f) - (num4 / 2f)));
            pe.Graphics.DrawLine(pen, new PointF(x, num * 6f), new PointF(x * 3f, (num * 6f) - num4));
            pe.Graphics.DrawLine(pen, new PointF(x, num * 10f), new PointF(x * 3f, (num * 10f) - num4));
            pen.DashStyle = DashStyle.Dash;
            pe.Graphics.DrawLine(pen, new PointF(x * 2f, (num * 2f) - (num4 / 2f)), new PointF(x * 2f, (num * 10f) - (num4 / 2f)));
        }

        private void Paint_StartButon(PaintEventArgs pe)
        {
            int num4;
            Pen magenta;
            Brush black;
            float num = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                black = Brushes.Magenta;
                num4 = 0;
            }
            else
            {
                magenta = Pens.Black;
                black = Brushes.Black;
                num4 = 3;
            }
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(x * 3f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x, num * 2f), this.r));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x * 3f, num * 2f), this.r));
            pe.Graphics.DrawLine(magenta, new PointF(x, ((num * 2f) - this.r) - num4), new PointF(x * 3f, ((num * 2f) - this.r) - num4));
            pe.Graphics.DrawLine(magenta, new PointF(x * 2f, 0f), new PointF(x * 2f, ((num * 2f) - this.r) - num4));
        }

        private void Paint_StartStopButon(PaintEventArgs pe)
        {
            float num5;
            Pen magenta;
            Brush black;
            int num6 = 9;
            float num = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 4f;
            float height = this.mContSize.Height;
            float width = this.mContSize.Width;
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                black = Brushes.Magenta;
                num5 = height - (this.r * 2f);
            }
            else
            {
                magenta = Pens.Black;
                black = Brushes.Black;
                num5 = (num * 2f) + this.r;
            }
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(x * 3f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x, num * 2f), this.r));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x * 3f, num * 2f), this.r));
            pe.Graphics.DrawLine(magenta, new PointF(0f, height - this.r), new PointF(x, height - this.r));
            pe.Graphics.DrawLine(magenta, new PointF(x * 3f, height - this.r), new PointF(width, height - this.r));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x, height - this.r), this.r));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x * 3f, height - this.r), this.r));
            pe.Graphics.DrawLine(magenta, new PointF(x, num5), new PointF(x * 3f, num5));
            pe.Graphics.DrawLine(magenta, new PointF(x * 2f, num5 - num6), new PointF(x * 2f, num5));
            if (this.Type == ElementType.SinirAnahtar2)
            {
                pe.Graphics.DrawEllipse(magenta, this.sqrRect(new PointF(x * 2f, num5 - num6), num / 2f));
            }
        }

        private void Paint_Startx3(PaintEventArgs pe)
        {
            float num4;
            Brush magenta;
            Pen pen = new Pen(Color.Black);
            float num = ((float) this.mContSize.Height) / 12f;
            float x = ((float) this.mContSize.Width) / 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            int num6 = 7;
            int num5 = 3;
            if (this.State == 2)
            {
                pen.Color = Color.Magenta;
                magenta = Brushes.Magenta;
                num4 = 0f;
            }
            else
            {
                pen.Color = Color.Black;
                magenta = Brushes.Black;
                num4 = this.r + num5;
            }
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 6f), new PointF(x, num * 6f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, num * 6f), new PointF(width, num * 6f));
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 10f), new PointF(x, num * 10f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, num * 10f), new PointF(width, num * 10f));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x, num * 2f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, num * 2f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x, num * 6f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, num * 6f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x, num * 10f), this.r));
            pe.Graphics.FillEllipse(magenta, this.sqrRectF(new PointF(x * 3f, num * 10f), this.r));
            pe.Graphics.DrawLine(pen, new PointF(x, num * 2f), new PointF(x * 3f, (num * 2f) - num4));
            pe.Graphics.DrawLine(pen, new PointF(x, num * 6f), new PointF(x * 3f, (num * 6f) - num4));
            pe.Graphics.DrawLine(pen, new PointF(x, num * 10f), new PointF(x * 3f, (num * 10f) - num4));
            pe.Graphics.DrawLine(pen, new PointF(x * 2f, ((num * 2f) - (num4 / 2f)) - num6), new PointF(x * 2f, (num * 2f) - (num4 / 2f)));
            pen.DashStyle = DashStyle.Dash;
            pe.Graphics.DrawLine(pen, new PointF(x * 2f, (num * 2f) - (num4 / 2f)), new PointF(x * 2f, (num * 10f) - (num4 / 2f)));
        }

        private void Paint_StopButon(PaintEventArgs pe)
        {
            int num4;
            Pen magenta;
            Brush black;
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                black = Brushes.Magenta;
                num4 = 3;
            }
            else
            {
                magenta = Pens.Black;
                black = Brushes.Black;
                num4 = 0;
            }
            int num5 = 10;
            float num = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(x * 3f, num * 2f), new PointF(width, num * 2f));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x, num * 2f), this.r));
            pe.Graphics.FillEllipse(black, this.sqrRectF(new PointF(x * 3f, num * 2f), this.r));
            pe.Graphics.DrawLine(magenta, new PointF(x, ((num * 2f) + this.r) + num4), new PointF(x * 3f, ((num * 2f) + this.r) + num4));
            pe.Graphics.DrawLine(magenta, new PointF(x * 2f, (((num * 2f) + this.r) + num4) - num5), new PointF(x * 2f, ((num * 2f) + this.r) + num4));
        }

        private void Paint_TersZamanRolesi(PaintEventArgs pe)
        {
            Pen magenta;
            Brush black;
            if ((this.State == 2) || (this.State == 3))
            {
                magenta = Pens.Magenta;
                black = Brushes.Magenta;
            }
            else
            {
                magenta = Pens.Black;
                black = Brushes.Black;
            }
            float num = ((float) this.mContSize.Height) / 4f;
            float height = ((float) this.mContSize.Height) / 3f;
            float x = ((float) this.mContSize.Width) / 4f;
            pe.Graphics.DrawLine(magenta, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(magenta, new PointF(x * 3f, num * 2f), new PointF(x * 4f, num * 2f));
            pe.Graphics.DrawRectangle(magenta, x, 0f, x * 2f, num * 4f);
            pe.Graphics.FillRectangle(black, x, 0f, x * 2f, height);
        }

        private void Paint_UcFazGuc(PaintEventArgs pe)
        {
            Pen black = Pens.Black;
            Brush brush = Brushes.Black;
            Rectangle rectangle = new Rectangle(0, 0, 10, base.Size.Height - 1);
            float num2 = ((float) this.mContSize.Height) / 3f;
            float single1 = ((float) this.mContSize.Height) / 2f;
            float x = ((float) this.mContSize.Width) / 6f;
            float r = x / 2f;
            float height = rectangle.Height;
            pe.Graphics.DrawString("R", this.Font, brush, (float) 0f, (float) 0f);
            PointF o = new PointF(x, num2 * 2f);
            pe.Graphics.DrawEllipse(black, this.sqrRect(o, r));
            pe.Graphics.DrawLine(black, new PointF(x, (num2 * 2f) + r), new PointF(x, height));
            pe.Graphics.DrawString("S", this.Font, brush, (float) (0f + (x * 2f)), (float) 0f);
            o = new PointF(x * 3f, num2 * 2f);
            pe.Graphics.DrawEllipse(black, this.sqrRect(o, r));
            pe.Graphics.DrawLine(black, new PointF(x * 3f, (num2 * 2f) + r), new PointF(x * 3f, height));
            pe.Graphics.DrawString("T", this.Font, brush, (float) (0f + (x * 4f)), (float) 0f);
            o = new PointF(x * 5f, num2 * 2f);
            pe.Graphics.DrawEllipse(black, this.sqrRect(o, r));
            pe.Graphics.DrawLine(black, new PointF(x * 5f, (num2 * 2f) + r), new PointF(x * 5f, height));
        }

        private void Paint_UcFazliMotor(PaintEventArgs pe)
        {
            Pen magenta;
            Brush green;
            float single1 = ((float) this.mContSize.Height) / 4f;
            float x = ((float) this.mContSize.Width) / 2f;
            float y = ((float) this.mContSize.Height) / 3f;
            float single2 = ((float) this.mContSize.Width) / 3f;
            float r = y - 1f;
            float num5 = 4f;
            int height = this.mContSize.Height;
            float width = this.mContSize.Width;
            PointF o = new PointF(x, y * 2f);
            PointF tf2 = new PointF(o.X + ((float) (Math.Cos((double) this.angle) * r)), o.Y + ((float) (Math.Sin((double) this.angle) * r)));
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                green = Brushes.Green;
            }
            else if (this.State == 1)
            {
                magenta = Pens.Black;
                green = Brushes.Black;
            }
            else
            {
                magenta = Pens.Black;
                green = Brushes.Black;
            }
            pe.Graphics.DrawLine(magenta, new PointF(x, 0f), new PointF(x, y));
            pe.Graphics.DrawEllipse(magenta, this.sqrRectF(o, r));
            if (this.State != 0)
            {
                pe.Graphics.FillEllipse(green, this.sqrRectF(tf2, num5));
            }
            PointF[] tfArray2 = new PointF[] { new PointF(0f, 0f), new PointF(0f, y), new PointF((((float) Math.Sin(3.9269908169872414)) * r) + o.X, (((float) Math.Cos(3.9269908169872414)) * r) + o.Y) };
            PointF[] points = tfArray2;
            pe.Graphics.DrawLines(magenta, points);
            tfArray2 = new PointF[] { new PointF(width, 0f), new PointF(width, y), new PointF((((float) Math.Sin(2.3561944901923448)) * r) + o.X, (((float) Math.Cos(2.3561944901923448)) * r) + o.Y) };
            points = tfArray2;
            pe.Graphics.DrawLines(magenta, points);
            pe.Graphics.DrawString("M", this.Font, Brushes.Black, (float) (o.X - 5f), (float) (o.Y - 15f));
            pe.Graphics.DrawString("3 ~", this.Font, Brushes.Black, o.X - 10f, o.Y);
        }

        private void Paint_UcFazliMotor2(PaintEventArgs pe)
        {
            Pen magenta;
            Brush green;
            float y = ((float) this.mContSize.Height) / 4f;
            float num2 = ((float) this.mContSize.Width) / 7f;
            float r = y * 2f;
            float num4 = 4f;
            Brush black = Brushes.Black;
            int height = this.mContSize.Height;
            int width = this.mContSize.Width;
            PointF o = new PointF(num2 * 5f, y * 2f);
            PointF tf2 = new PointF(o.X + ((float) (Math.Cos((double) this.angle) * r)), o.Y + ((float) (Math.Sin((double) this.angle) * r)));
            if (this.State == 2)
            {
                magenta = Pens.Magenta;
                green = Brushes.Green;
            }
            else if (this.State == 1)
            {
                magenta = Pens.Black;
                green = Brushes.Black;
            }
            else
            {
                magenta = Pens.Black;
                green = Brushes.Black;
            }
            pe.Graphics.DrawLine(magenta, new PointF(num2 / 2f, 0f), new PointF(num2 / 2f, y));
            pe.Graphics.DrawLine(magenta, new PointF((num2 * 3f) / 2f, 0f), new PointF((num2 * 3f) / 2f, y));
            pe.Graphics.DrawLine(magenta, new PointF((num2 * 5f) / 2f, 0f), new PointF((num2 * 5f) / 2f, y));
            pe.Graphics.DrawLine(magenta, new PointF(num2 / 2f, y * 3f), new PointF(num2 / 2f, y * 4f));
            pe.Graphics.DrawLine(magenta, new PointF((num2 * 3f) / 2f, y * 3f), new PointF((num2 * 3f) / 2f, y * 4f));
            pe.Graphics.DrawLine(magenta, new PointF((num2 * 5f) / 2f, y * 3f), new PointF((num2 * 5f) / 2f, y * 4f));
            pe.Graphics.DrawEllipse(magenta, this.sqrRectF(o, r));
            if (this.State != 0)
            {
                pe.Graphics.FillEllipse(green, this.sqrRectF(tf2, num4));
            }
            PointF[] points = new PointF[] { new PointF(o.X - ((float) (r * Math.Sin(Math.Acos((double) (y / r))))), y), new PointF(0f, y), new PointF(0f, y * 3f), new PointF(o.X - ((float) (r * Math.Sin(Math.Acos((double) (y / r))))), y * 3f) };
            pe.Graphics.DrawLines(magenta, points);
            pe.Graphics.DrawString("M", this.Font, green, (float) (o.X - 5f), (float) (o.Y - 15f));
            pe.Graphics.DrawString("3 ~", this.Font, green, o.X - 10f, o.Y);
            Font font = new Font(this.Font.FontFamily.Name, 6f);
            pe.Graphics.DrawString("U", font, black, (float) 2f, (float) (y + 1f));
            pe.Graphics.DrawString("V", font, black, (float) ((num2 * 1f) + 2f), (float) (y + 1f));
            pe.Graphics.DrawString("W", font, black, (float) ((num2 * 2f) + 2f), (float) (y + 1f));
            pe.Graphics.DrawString("Z", font, black, (float) 2f, (float) ((y * 2f) + 1f));
            pe.Graphics.DrawString("X", font, black, (float) ((num2 * 1f) + 2f), (float) ((y * 2f) + 1f));
            pe.Graphics.DrawString("Y", font, black, (float) ((num2 * 2f) + 2f), (float) ((y * 2f) + 1f));
        }

        private void Paint_ZamanRolesi(PaintEventArgs pe)
        {
            Pen pen;
            if ((this.State == 2) || (this.State == 3))
            {
                pen = new Pen(Color.Magenta);
            }
            else
            {
                pen = new Pen(Color.Black);
            }
            float num = ((float) this.mContSize.Height) / 4f;
            float y = ((float) this.mContSize.Height) / 3f;
            float x = ((float) this.mContSize.Width) / 4f;
            pe.Graphics.DrawLine(pen, new PointF(0f, num * 2f), new PointF(x, num * 2f));
            pe.Graphics.DrawLine(pen, new PointF(x * 3f, num * 2f), new PointF(x * 4f, num * 2f));
            pe.Graphics.DrawRectangle(pen, x, 0f, x * 2f, num * 4f);
            PointF[] points = new PointF[] { new PointF(x, 0f), new PointF(x * 3f, y), new PointF(x, y), new PointF(x * 3f, 0f) };
            pe.Graphics.DrawLines(pen, points);
        }

        public void PaintToWinForm(PaintEventArgs pe)
        {
            pe.Graphics.TranslateTransform((float) base.Location.X, (float) base.Location.Y, MatrixOrder.Append);
            this.OnPaint(pe);
            pe.Graphics.TranslateTransform((float) -base.Location.X, (float) -base.Location.Y, MatrixOrder.Append);
            if (this.label1.Text != "...")
            {
                pe.Graphics.DrawString(this.label1.Text, this.label1.Font, Brushes.Black, (PointF) this.label1.Location);
            }
            if (this.label2.Enabled)
            {
                pe.Graphics.DrawString(this.label2.Text, this.label2.Font, Brushes.Black, (PointF) this.label2.Location);
            }
        }

        public void PrintPaint(PrintPageEventArgs e)
        {
            Rectangle clipRect = new Rectangle(new Point(0, 0), this.mContSize);
            PaintEventArgs pe = new PaintEventArgs(e.Graphics, clipRect);
            this.PaintToWinForm(pe);
        }

        private Point RelPoint(Point p)
        {
            switch (this.iRotation)
            {
                case 0:
                    return p;

                case 1:
                    return new Point(this.pShift.X - p.Y, p.X);

                case 2:
                    return new Point(this.pShift.X - p.X, this.pShift.Y - p.Y);

                case 3:
                    return new Point(p.Y, this.pShift.Y - p.X);
            }
            MessageBox.Show("Farkli iRotation degeri kullailmis. Elements.RelPoint()");
            return p;
        }

        private CPoint.Sides RelSide(CPoint.Sides s)
        {
            CPoint.Sides sides = s;
            for (int i = 0; i < this.iRotation; i++)
            {
                sides = this.NextCPSide(sides);
            }
            return sides;
        }

        public void ResetModify()
        {
            for (int i = 0; i < this.mCPoint.Length; i++)
            {
                this.mCPoint[i].modif = false;
            }
        }

        private void scanTimer_Tick(object sender, EventArgs e)
        {
        }

        private void ScheduleChangeContacts(int iState)
        {
            ArrayList list = new ArrayList();
            list.Add(this);
            list.Add(iState);
            if ((base.Parent.Parent != null) && (base.Parent.Parent.GetType() == typeof(frmMain)))
            {
                ((frmMain) base.Parent.Parent).alChangeContacts.Add(list);
            }
            else
            {
                MessageBox.Show("hata: gercek degisken bulunamadi! 'Elements.ScheduleChangeContacts()' ");
            }
            this.frmMain_c1ScanCircuit();
        }

        public Rectangle sqrRect(PointF o, float r)
        {
            return new Rectangle((int) (o.X - r), (int) (o.Y - r), (int) (r * 2f), (int) (r * 2f));
        }

        public RectangleF sqrRectF(PointF o, float r)
        {
            return new RectangleF(o.X - r, o.Y - r, r * 2f, r * 2f);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float num;
            int num3;
            bool flag = false;
            switch (this.Type)
            {
                case ElementType.ZamanRolesi:
                    this.int2--;
                    if (this.int2 <= 0)
                    {
                        this.int2 = 0;
                        this.timer1.Enabled = false;
                        this.ScheduleChangeContacts(2);
                    }
                    this.ChangeContactsCounter(this.int2.ToString());
                    return;

                case ElementType.TersZamanRolesi:
                    if (this.State != 2)
                    {
                        this.int2--;
                        if (this.int2 <= 0)
                        {
                            this.int2 = 0;
                            this.timer1.Enabled = false;
                            this.ScheduleChangeContacts(1);
                        }
                        this.ChangeContactsCounter(this.int2.ToString());
                    }
                    return;

                case ElementType.UcFazliMotor:
                case ElementType.UcFazliMotor2:
                    if (this.State == 1)
                    {
                        this.int2--;
                    }
                    num = (this.int1 * this.int2) / 10f;
                    this.angle += num;
                    this.angle = this.angle % 6.283185f;
                    this.Refresh();
                    if (this.int2 <= 0)
                    {
                        this.timer1.Enabled = false;
                        this.int1 = 0;
                        this.int2 = 0;
                    }
                    return;

                case ElementType.BirFazMotor:
                    if (((this.State != 1) && (this.State != 2)) && (this.State != 4))
                    {
                        if (this.State == 5)
                        {
                            if (this.mCnt >= 30)
                            {
                                this.frmMain_c1AlertElement(0);
                            }
                            this.mCnt++;
                        }
                    }
                    else
                    {
                        this.int2--;
                    }
                    num = (this.int1 * this.int2) / 10f;
                    this.angle += num;
                    this.angle = this.angle % 6.283185f;
                    this.Refresh();
                    if (this.int2 <= 0)
                    {
                        this.timer1.Enabled = false;
                        this.int1 = 0;
                        this.int2 = 0;
                    }
                    return;

                case ElementType.SinirAnahtar2:
                case ElementType.UcAcikKontak:
                case ElementType.UcKapaliKontak:
                    return;

                case ElementType.BirFazMotor2:
                    if (((this.State != 1) && (this.State != 2)) && (this.State != 4))
                    {
                        if (this.State == 5)
                        {
                            if (this.mCnt >= 10)
                            {
                                this.State = 3;
                            }
                            this.mCnt++;
                        }
                    }
                    else
                    {
                        this.int2--;
                    }
                    num = (this.int1 * this.int2) / 10f;
                    this.angle += num;
                    this.angle = this.angle % 6.283185f;
                    this.Refresh();
                    if (this.int2 <= 0)
                    {
                        this.timer1.Enabled = false;
                        this.int1 = 0;
                        this.int2 = 0;
                    }
                    return;

                case ElementType.Kapi1:
                case ElementType.Kapi2:
                    num3 = ((int[]) this.htSysVars["kapi"])[4];
                    if (!((bool) this.htSysState["p1"]))
                    {
                        this.timer1.Enabled = false;
                        goto Label_022C;
                    }
                    if (!((bool) this.htSysState["p3"]))
                    {
                        if ((bool) this.htSysState["p2"])
                        {
                            if (num3 == 1)
                            {
                                this.mCnt++;
                                if (this.mCnt == 5)
                                {
                                    this.frmMain_c1AlertElement(1);
                                }
                            }
                        }
                        else
                        {
                            this.mCnt = 0;
                        }
                        break;
                    }
                    if (num3 == 1)
                    {
                        this.mCnt++;
                        if (this.mCnt == 5)
                        {
                            this.frmMain_c1AlertElement(1);
                        }
                    }
                    break;

                case ElementType.Asansor:
                {
                    int num4 = ((int[]) this.htSysVars["asansor"])[4];
                    if (!((bool) this.htSysState["p1"]))
                    {
                        this.timer1.Enabled = false;
                    }
                    else
                    {
                        if (!((bool) this.htSysState["p6"]))
                        {
                            if ((bool) this.htSysState["p2"])
                            {
                                if (num4 == -1)
                                {
                                    this.mCnt++;
                                    if (this.mCnt == 5)
                                    {
                                        this.frmMain_c1AlertElement(1);
                                    }
                                }
                            }
                            else
                            {
                                this.mCnt = 0;
                            }
                        }
                        else if (num4 == 1)
                        {
                            this.mCnt++;
                            if (this.mCnt == 5)
                            {
                                this.frmMain_c1AlertElement(1);
                            }
                        }
                        flag = this.State != 0;
                    }
                    if (flag)
                    {
                        num = (num4 * 5f) / 10f;
                        this.angle += num;
                        this.angle = this.angle % 6.283185f;
                        ((int[]) this.htSysVars["asansor"])[0] += num4 * ((int[]) this.htSysVars["asansor"])[3];
                        if (((int[]) this.htSysVars["asansor"])[0] <= ((int[]) this.htSysVars["asansor"])[2])
                        {
                            ((int[]) this.htSysVars["asansor"])[0] = ((int[]) this.htSysVars["asansor"])[2];
                        }
                        else if (((int[]) this.htSysVars["asansor"])[0] >= ((int[]) this.htSysVars["asansor"])[1])
                        {
                            ((int[]) this.htSysVars["asansor"])[0] = ((int[]) this.htSysVars["asansor"])[1];
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            int num5 = ((int[]) this.htSysVars["asansor"])[0];
                            if (num5 == ((40 * (i + 1)) + 5))
                            {
                                this.htSysStateSet("p" + (i + 2), true);
                            }
                            else
                            {
                                this.htSysStateSet("p" + (i + 2), false);
                            }
                        }
                        this.changeCPState();
                        this.Refresh();
                    }
                    return;
                }
                default:
                    return;
            }
            flag = this.State != 0;
        Label_022C:
            if (flag)
            {
                num = (num3 * 5f) / 10f;
                this.angle += num;
                this.angle = this.angle % 6.283185f;
                ((int[]) this.htSysVars["kapi"])[0] += num3 * ((int[]) this.htSysVars["kapi"])[3];
                if (((int[]) this.htSysVars["kapi"])[0] <= ((int[]) this.htSysVars["kapi"])[2])
                {
                    ((int[]) this.htSysVars["kapi"])[0] = ((int[]) this.htSysVars["kapi"])[2];
                    this.htSysStateSet("p2", true);
                }
                else if (((int[]) this.htSysVars["kapi"])[0] >= ((int[]) this.htSysVars["kapi"])[1])
                {
                    ((int[]) this.htSysVars["kapi"])[0] = ((int[]) this.htSysVars["kapi"])[1];
                    this.htSysStateSet("p3", true);
                }
                else
                {
                    this.htSysStateSet("p2", false);
                    this.htSysStateSet("p3", false);
                }
                this.changeCPState();
                this.Refresh();
            }
        }

        public bool Alert
        {
            set
            {
                if (value)
                {
                    this.alertTimer.Enabled = true;
                }
                else
                {
                    this.alertTimer.Enabled = false;
                    this.BackColor = base.Parent.BackColor;
                }
            }
        }

        public bool modifyWay
        {
            get
            {
                return this.bModifyWay;
            }
            set
            {
                if (!value)
                {
                    for (int i = 0; i < this.mCPointLength(); i++)
                    {
                        this.oCPoint(i).way = null;
                    }
                }
                this.bModifyWay = value;
            }
        }

        public bool Movable
        {
            get
            {
                return this.mMovable;
            }
            set
            {
                this.mMovable = value;
            }
        }

        public Point oPoint
        {
            get
            {
                return this.moPoint;
            }
            set
            {
                this.moPoint = value;
            }
        }

        public int Rotation
        {
            get
            {
                return this.iRotation;
            }
            set
            {
                value = value % 4;
                if (this.State == 0)
                {
                    this.iRotation = value;
                    switch (this.iRotation)
                    {
                        case 0:
                            this.fDegree = 0f;
                            this.pShift = new Point(0, 0);
                            base.Size = new Size(this.mContSize.Width + 1, this.mContSize.Height + 1);
                            break;

                        case 1:
                            this.fDegree = 90f;
                            this.pShift = new Point(this.mContSize.Height, 0);
                            base.Size = new Size(this.mContSize.Height + 1, this.mContSize.Width + 1);
                            break;

                        case 2:
                            this.fDegree = 180f;
                            this.pShift = new Point(this.mContSize.Width, this.mContSize.Height);
                            base.Size = new Size(this.mContSize.Width + 1, this.mContSize.Height + 1);
                            break;

                        case 3:
                            this.fDegree = 270f;
                            this.pShift = new Point(0, this.mContSize.Width);
                            base.Size = new Size(this.mContSize.Height + 1, this.mContSize.Width + 1);
                            break;
                    }
                    this.InitCPoint();
                    this.Refresh();
                }
            }
        }

        public bool Selected
        {
            get
            {
                return this.mSelected;
            }
            set
            {
                this.mSelected = value;
                if (value)
                {
                    this.BackColor = Color.FromArgb(220, 220, 220);
                    base.BringToFront();
                    this.label1.BackColor = Color.FromArgb(220, 220, 220);
                    this.label1.BringToFront();
                    if (this.label2.Enabled)
                    {
                        this.label2.BackColor = Color.FromArgb(220, 220, 220);
                        this.label2.BringToFront();
                    }
                }
                else
                {
                    if (base.Parent != null)
                    {
                        this.BackColor = base.Parent.BackColor;
                    }
                    this.label1.BackColor = Color.Transparent;
                    if (this.label2.Enabled)
                    {
                        this.label2.BackColor = Color.Transparent;
                    }
                }
                for (int i = 0; i < this.mCPoint.Length; i++)
                {
                    if ((this.mCPoint[i].oCon != null) && (this.mCPoint[i].oCon.ToString() == "ESCS.Line"))
                    {
                        ((Line) this.mCPoint[i].oCon).Selected = value;
                    }
                }
            }
        }

        public int State
        {
            get
            {
                return this.mState;
            }
            set
            {
                int num;
                if (value >= this.mStateCount)
                {
                    value = this.mStateCount - 1;
                }
                if ((this.mState == value) || !this.Movable)
                {
                    return;
                }
                this.mState = value;
                if ((value != 0) && this.Selected)
                {
                    this.Selected = false;
                }
                if (value == 0)
                {
                    this.modifyWay = false;
                    this.ResetModify();
                    this.Alert = false;
                }
                switch (this.Type)
                {
                    case ElementType.Role:
                        this.ScheduleChangeContacts(value);
                        break;

                    case ElementType.ZamanRolesi:
                        switch (value)
                        {
                            case 0:
                                this.timer1.Enabled = false;
                                this.label2.Text = this.int1.ToString();
                                this.int2 = this.int1;
                                this.ScheduleChangeContacts(0);
                                this.ChangeContactsCounter(this.label2.Text);
                                goto Label_0DD2;

                            case 1:
                                this.timer1.Enabled = false;
                                this.int2 = this.int1;
                                this.ScheduleChangeContacts(1);
                                this.ChangeContactsCounter(this.int1.ToString());
                                goto Label_0DD2;

                            case 2:
                                this.ScheduleChangeContacts(1);
                                this.timer1.Enabled = true;
                                goto Label_0DD2;
                        }
                        break;

                    case ElementType.TersZamanRolesi:
                        switch (value)
                        {
                            case 0:
                                this.timer1.Enabled = false;
                                this.ScheduleChangeContacts(0);
                                this.label2.Text = this.int1.ToString();
                                this.ChangeContactsCounter(this.label2.Text);
                                goto Label_0DD2;

                            case 2:
                                this.int2 = this.int1;
                                this.ChangeContactsCounter(this.label2.Text);
                                this.ScheduleChangeContacts(2);
                                this.timer1.Enabled = true;
                                goto Label_0DD2;
                        }
                        break;

                    case ElementType.AcikKontak:
                    case ElementType.StartButon:
                        switch (value)
                        {
                            case 1:
                                this.mCPoint[0].arrCP = new CPoint[0];
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;

                            case 2:
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                                this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;
                        }
                        this.mCPoint[0].arrCP = new CPoint[0];
                        this.mCPoint[1].arrCP = new CPoint[0];
                        break;

                    case ElementType.KapaliKontak:
                    case ElementType.StopButon:
                        switch (value)
                        {
                            case 1:
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                                this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;

                            case 2:
                                this.mCPoint[0].arrCP = new CPoint[0];
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;
                        }
                        this.mCPoint[0].arrCP = new CPoint[0];
                        this.mCPoint[1].arrCP = new CPoint[0];
                        break;

                    case ElementType.StartStopButon:
                    case ElementType.SinirAnahtar2:
                        switch (value)
                        {
                            case 1:
                                this.mCPoint[0].arrCP = new CPoint[0];
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.mCPoint[2].arrCP = new CPoint[0];
                                this.mCPoint[3].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                                this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.mCPoint[2].arrCP = new CPoint[0];
                                this.mCPoint[3].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;

                            case 2:
                                this.mCPoint[0].arrCP = new CPoint[0];
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.mCPoint[2].arrCP = new CPoint[0];
                                this.mCPoint[3].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                this.mCPoint[0].arrCP = new CPoint[0];
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[3] };
                                this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[2] };
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;
                        }
                        break;

                    case ElementType.SinirAnahtar:
                        switch (value)
                        {
                            case 1:
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[2] };
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;

                            case 2:
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                                this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.mCPoint[2].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;
                        }
                        break;

                    case ElementType.UcFazliMotor:
                        switch (value)
                        {
                            case 1:
                                this.timer1.Enabled = true;
                                goto Label_0DD2;

                            case 2:
                                this.int2 = 10;
                                this.timer1.Enabled = true;
                                goto Label_0DD2;
                        }
                        this.timer1.Enabled = false;
                        for (num = 0; num < this.mCPointLength(); num++)
                        {
                            this.oCPoint(num).way = null;
                        }
                        this.int1 = 0;
                        this.int2 = 0;
                        break;

                    case ElementType.Startx3:
                    case ElementType.UcAcikKontak:
                        switch (value)
                        {
                            case 1:
                                this.mCPoint[0].arrCP = new CPoint[0];
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.mCPoint[2].arrCP = new CPoint[0];
                                this.mCPoint[3].arrCP = new CPoint[0];
                                this.mCPoint[4].arrCP = new CPoint[0];
                                this.mCPoint[5].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;

                            case 2:
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                                this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[3] };
                                this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[2] };
                                this.mCPoint[4].arrCP = new CPoint[] { this.mCPoint[5] };
                                this.mCPoint[5].arrCP = new CPoint[] { this.mCPoint[4] };
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;
                        }
                        break;

                    case ElementType.Start3x3:
                        switch (value)
                        {
                            case 1:
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                                this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.mCPoint[2].arrCP = new CPoint[0];
                                this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[4] };
                                this.mCPoint[4].arrCP = new CPoint[] { this.mCPoint[3] };
                                this.mCPoint[5].arrCP = new CPoint[0];
                                this.mCPoint[6].arrCP = new CPoint[] { this.mCPoint[7] };
                                this.mCPoint[7].arrCP = new CPoint[] { this.mCPoint[6] };
                                this.mCPoint[8].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;

                            case 2:
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[2] };
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[5] };
                                this.mCPoint[4].arrCP = new CPoint[0];
                                this.mCPoint[5].arrCP = new CPoint[] { this.mCPoint[3] };
                                this.mCPoint[6].arrCP = new CPoint[] { this.mCPoint[8] };
                                this.mCPoint[7].arrCP = new CPoint[0];
                                this.mCPoint[8].arrCP = new CPoint[] { this.mCPoint[6] };
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;
                        }
                        break;

                    case ElementType.BirFazMotor:
                    case ElementType.BirFazMotor2:
                        switch (value)
                        {
                            case 1:
                            case 2:
                            case 4:
                                this.mCnt = 0;
                                this.timer1.Enabled = true;
                                goto Label_0DD2;

                            case 3:
                                this.int2 = 10;
                                this.mCnt = 0;
                                this.timer1.Enabled = true;
                                goto Label_0DD2;

                            case 5:
                                this.int2 = 10;
                                this.timer1.Enabled = true;
                                goto Label_0DD2;
                        }
                        this.mCnt = 0;
                        this.timer1.Enabled = false;
                        this.int1 = 0;
                        this.int2 = 0;
                        break;

                    case ElementType.UcFazliMotor2:
                        switch (value)
                        {
                            case 1:
                                this.timer1.Enabled = true;
                                for (num = 0; num < this.mCPointLength(); num++)
                                {
                                    this.oCPoint(num).way = null;
                                }
                                goto Label_0DD2;

                            case 2:
                                this.int2 = 10;
                                this.timer1.Enabled = true;
                                goto Label_0DD2;
                        }
                        this.timer1.Enabled = false;
                        for (num = 0; num < this.mCPointLength(); num++)
                        {
                            this.oCPoint(num).way = null;
                        }
                        this.int1 = 0;
                        this.int2 = 0;
                        break;

                    case ElementType.UcKapaliKontak:
                        switch (value)
                        {
                            case 1:
                                this.mCPoint[0].arrCP = new CPoint[] { this.mCPoint[1] };
                                this.mCPoint[1].arrCP = new CPoint[] { this.mCPoint[0] };
                                this.mCPoint[2].arrCP = new CPoint[] { this.mCPoint[3] };
                                this.mCPoint[3].arrCP = new CPoint[] { this.mCPoint[2] };
                                this.mCPoint[4].arrCP = new CPoint[] { this.mCPoint[5] };
                                this.mCPoint[5].arrCP = new CPoint[] { this.mCPoint[4] };
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;

                            case 2:
                                this.mCPoint[0].arrCP = new CPoint[0];
                                this.mCPoint[1].arrCP = new CPoint[0];
                                this.mCPoint[2].arrCP = new CPoint[0];
                                this.mCPoint[3].arrCP = new CPoint[0];
                                this.mCPoint[4].arrCP = new CPoint[0];
                                this.mCPoint[5].arrCP = new CPoint[0];
                                this.frmMain_c1ScanCircuit();
                                goto Label_0DD2;
                        }
                        break;

                    case ElementType.Kapi1:
                    case ElementType.Kapi2:
                        this.htSysStateSet("p1", false);
                        this.htSysStateSet("p2", true);
                        this.htSysStateSet("p3", false);
                        this.changeCPState();
                        if (value == 0)
                        {
                            this.timer1.Enabled = false;
                            ((int[]) this.htSysVars["kapi"])[0] = ((int[]) this.htSysVars["kapi"])[2];
                            this.mCnt = 0;
                        }
                        break;

                    case ElementType.Asansor:
                        for (num = 1; num < 6; num++)
                        {
                            this.htSysStateSet("p" + num, false);
                        }
                        this.htSysStateSet("p6", true);
                        this.changeCPState();
                        if (value == 0)
                        {
                            this.timer1.Enabled = false;
                            ((int[]) this.htSysVars["asansor"])[0] = ((int[]) this.htSysVars["asansor"])[1];
                            this.mCnt = 0;
                        }
                        break;
                }
            Label_0DD2:
                if (this.bHand && (this.State != 0))
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
                this.Refresh();
            }
        }

        public ElementType Type
        {
            get
            {
                return this.mType;
            }
            set
            {
                this.mType = value;
                this.Text = this.mType.ToString();
                switch (value)
                {
                    case ElementType.Role:
                        this.bHand = true;
                        this.mContSize = new Size(20, 20);
                        this.mStateCount = 3;
                        this.timer1.Interval = 0x3e8;
                        break;

                    case ElementType.ZamanRolesi:
                    case ElementType.TersZamanRolesi:
                        this.mContSize = new Size(20, 20);
                        this.mStateCount = 3;
                        this.timer1.Interval = 0x3e8;
                        break;

                    case ElementType.AcikKontak:
                    case ElementType.KapaliKontak:
                        this.mContSize = new Size(20, 20);
                        this.mStateCount = 3;
                        break;

                    case ElementType.StartButon:
                    case ElementType.StopButon:
                    case ElementType.StartStopButon:
                    case ElementType.SinirAnahtar:
                    case ElementType.SinirAnahtar2:
                        this.mContSize = new Size(20, 20);
                        this.mStateCount = 3;
                        this.bHand = true;
                        break;

                    case ElementType.Lamba:
                        this.mColor = Color.Magenta;
                        this.mContSize = new Size(20, 20);
                        this.mStateCount = 3;
                        this.timer1.Interval = 0x3e8;
                        break;

                    case ElementType.UcFazliMotor:
                        this.mContSize = new Size(40, 60);
                        this.mStateCount = 4;
                        this.timer1.Interval = 100;
                        this.iGroup = 1;
                        break;

                    case ElementType.GucR:
                    case ElementType.GucS:
                    case ElementType.GucT:
                    case ElementType.GucN:
                        this.mContSize = new Size(10, 30);
                        this.mStateCount = 2;
                        this.iGroup = 2;
                        break;

                    case ElementType.Startx3:
                    case ElementType.Start3x3:
                        this.mContSize = new Size(20, 60);
                        this.mStateCount = 3;
                        this.bHand = true;
                        break;

                    case ElementType.UcFazGuc:
                        this.mContSize = new Size(30, 30);
                        this.mStateCount = 2;
                        this.iGroup = 2;
                        break;

                    case ElementType.BirFazMotor:
                    case ElementType.BirFazMotor2:
                        this.mContSize = new Size(80, 100);
                        this.mStateCount = 6;
                        this.timer1.Interval = 100;
                        this.iGroup = 1;
                        break;

                    case ElementType.UcFazliMotor2:
                        this.mContSize = new Size(70, 40);
                        this.mStateCount = 3;
                        this.timer1.Interval = 100;
                        this.iGroup = 1;
                        break;

                    case ElementType.UcAcikKontak:
                    case ElementType.UcKapaliKontak:
                        this.mContSize = new Size(20, 70);
                        this.mStateCount = 3;
                        break;

                    case ElementType.Kapi1:
                        this.mContSize = new Size(280, 100);
                        this.timer1.Interval = 100;
                        this.iGroup = 3;
                        this.mStateCount = 2;
                        this.htSysState = new Hashtable();
                        this.htSysState.Add("p1", false);
                        this.htSysState.Add("p2", false);
                        this.htSysState.Add("p3", false);
                        this.htSysVars = new Hashtable();
                        this.htSysVars.Add("kapi", new int[] { 20, 130, 20, 5, 0 });
                        break;

                    case ElementType.Kapi2:
                    {
                        this.mContSize = new Size(100, 100);
                        this.timer1.Interval = 100;
                        this.iGroup = 3;
                        this.mStateCount = 2;
                        this.htSysState = new Hashtable();
                        this.htSysState.Add("p1", false);
                        this.htSysState.Add("p2", false);
                        this.htSysState.Add("p3", false);
                        this.htSysVars = new Hashtable();
                        int[] numArray = new int[5];
                        numArray[1] = 90;
                        numArray[3] = 5;
                        this.htSysVars.Add("kapi", numArray);
                        break;
                    }
                    case ElementType.Asansor:
                        this.mContSize = new Size(60, 240);
                        this.timer1.Interval = 100;
                        this.iGroup = 3;
                        this.mStateCount = 2;
                        this.htSysState = new Hashtable();
                        this.htSysState.Add("p1", false);
                        this.htSysState.Add("p2", false);
                        this.htSysState.Add("p3", false);
                        this.htSysState.Add("p4", false);
                        this.htSysState.Add("p5", false);
                        this.htSysState.Add("p6", false);
                        this.htSysVars = new Hashtable();
                        this.htSysVars.Add("asansor", new int[] { 0xcd, 0xcd, 0x2d, 5, 0 });
                        break;

                    default:
                        MessageBox.Show("HATA: Belirtilen tipler haricindi bir tip kalmiyor.");
                        this.mContSize = new Size(20, 20);
                        break;
                }
                this.Rotation = 0;
                this.InitCPoint();
                this.Refresh();
            }
        }

        public enum ElementType
        {
            Role,
            ZamanRolesi,
            TersZamanRolesi,
            AcikKontak,
            KapaliKontak,
            StartButon,
            StopButon,
            StartStopButon,
            SinirAnahtar,
            Lamba,
            UcFazliMotor,
            GucR,
            GucS,
            GucT,
            GucN,
            Startx3,
            Start3x3,
            UcFazGuc,
            BirFazMotor,
            UcFazliMotor2,
            SinirAnahtar2,
            BirFazMotor2,
            UcAcikKontak,
            UcKapaliKontak,
            Kapi1,
            Kapi2,
            Asansor
        }
    }
}

