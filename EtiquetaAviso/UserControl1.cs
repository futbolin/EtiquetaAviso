using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace EtiquetaAviso
{
    public enum eMarca
    {
        Nada,
        Cruz,
        Circulo,
        Imagen
    }


    [DefaultProperty("Marca"), DefaultEvent("Load")]
    public partial class UserControl1 : Control
    {
        private int offsetX = 0,offsetY = 0,grosor = 0;
            
        private eMarca marca = eMarca.Nada;
        [Category("Apariencia")]
        [Description("Tipo de marca que aparece antes del texto")]
        public eMarca Marca
        {
            set
            {
                marca = value;
                this.Refresh();
            }
            get { return marca; }
        }

        
        private Boolean gradiente = false;
        [Category("Apariencia")]
        [Description("Indica si tiene fondo de gradiente")]
        public Boolean Gradiente
        {
            set
            {
                gradiente = value;
                this.Refresh();
            }
            get { return gradiente; }
        }

      
        private Color color1;
        [Category("Apariencia")]
        [Description("Color 1 en caso de que tenga gradiente")]
        public Color Color1
        {
            set
            {
                color1 = value;
                Refresh();
            }
            get { return color1; }
        }

        
        private Color color2;
        [Category("Apariencia")]
        [Description("Color 2 en caso de que tenga gradiente")]
        public Color Color2
        {
            set
            {
                color2 = value;
                Refresh();
            }
            get { return color2; }
        }

        private Image imagen;
        [Category("Apariencia")]
        [Description("Imagen que sustituye a la marca")]
        public Image Imagen
        {
            set
            {
                imagen = value;
                Refresh();
            }
            get { return imagen; }
        }
        public UserControl1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            if (gradiente)
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush(new PointF(0,0),new PointF(Width,Height),
                    color1,color2);

                g.FillRectangle(gradientBrush, new RectangleF(0, 0, Width, Height));
            }

          
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //Dependiendo del valor de la propiedad marca dibujamos una
            //Cruz o un Círculo
            switch (Marca)
            {
                case eMarca.Circulo:
                    grosor = 20;
                    g.DrawEllipse(new Pen(Color.Green, grosor), grosor, grosor,
                   this.Font.Height, this.Font.Height);
                    offsetX = this.Font.Height + grosor;
                    offsetY = grosor;
                    break;
                case eMarca.Cruz:
                    grosor = 5;
                    Pen lapiz = new Pen(Color.Red, grosor);
                    g.DrawLine(lapiz, grosor, grosor, this.Font.Height,
                   this.Font.Height);
                    g.DrawLine(lapiz, this.Font.Height, grosor, grosor,
                   this.Font.Height);
                    offsetX = this.Font.Height + grosor;
                    offsetY = grosor / 2;
                    //Es recomendable liberar recursos de dibujo pues se
                    //pueden realizar muchos y cogen memoria
                    lapiz.Dispose();
                    break;
                case eMarca.Imagen:
                   if(imagen != null)
                    {
                        grosor = 10;
                        g.DrawImage(imagen, 0, 0, Font.Height, Font.Height);
                        offsetX = Font.Height;
                    }
                    break;


            }
            //Finalmente pintamos el Texto; desplazado si fuera necesario
            SolidBrush b = new SolidBrush(this.ForeColor);
            g.DrawString(this.Text, this.Font, b, offsetX + grosor, offsetY);
            Size tam = g.MeasureString(this.Text, this.Font).ToSize();
            this.Size = new Size(tam.Width + offsetX + grosor, tam.Height + offsetY
           * 2);
            b.Dispose();
        }

     

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if(marca != eMarca.Nada && imagen != null)
            {
                if(((e.X - this.Location.X) < offsetX + grosor) && ((e.Y - this.Location.Y) < offsetY))
                {
                    MarcaPulsada?.Invoke(this, new EventArgs());
                }
            }
        }

        [Category("Marca Pulsada")]
        [Description("Se lanza cuando se pulsa la marca")]
        public event System.EventHandler MarcaPulsada;
    }
}
