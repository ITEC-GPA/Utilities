using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPC.Utilities.Graphics
{
    public class GraphicCreator : IDisposable
    {
        protected class Curve
        {
            protected string _name;
            protected Color _color;
            protected List<double[]> _values;

            public string Name
            {
                get { return _name; }
            }

            public Color Color
            {
                get { return _color; }
            }

            public List<double[]> Values
            {
                get { return _values; }
            }

            public Curve(string name, Color color)
            {
                _name = name;
                _color = color;
                _values = new List<double[]>();
            }
        }

        protected float _w_bmp;
        protected float _h_bmp;
        protected float[] _margins; // 0 = left, 1 = top, 2 = right, 3 = bottom
        protected double _max_x;
        protected double _min;
        protected double _max;
        protected string _title;
        protected List<double[]> _maximumValues;
        protected List<Curve> _curves;
        protected List<double> _abscissaReferences;
        protected Dictionary<double, Color> _referenceOrdinates;
        protected Dictionary<PointF, Color> _referencePoints;
        protected Bitmap _watermark;
        protected bool _drawLegend;

        public string Title
        {
            get => _title; 
            set => _title = value; 
        }

        /// <summary>
        /// References list in te abscissa axis
        /// </summary>
        public List<double> AbscissaReferences => _abscissaReferences; 

        public Bitmap Watermark
        {
            get => _watermark;
            set
            {
                _watermark?.Dispose();
                _watermark = null;

                if (value is null)
                    return;

                _watermark = (Bitmap)value.Clone();

                const byte ALPHA = 128;
                // Set the watermark's pixels' Alpha components.
                Color clr;
                for (int py = 0; py < _watermark.Height; py++)
                {
                    for (int px = 0; px < _watermark.Width; px++)
                    {
                        clr = _watermark.GetPixel(px, py);
                        _watermark.SetPixel(px, py, Color.FromArgb(ALPHA, clr.R, clr.G, clr.B));
                    }
                }

                // Set the watermark's transparent color.
                _watermark.MakeTransparent(_watermark.GetPixel(0, 0));
            }
        }

        public bool DrawLegend
        {
            get => _drawLegend;
            set => _drawLegend = value;
        }

        public GraphicCreator(float w_bmp, float h_bmp)
        {
            _w_bmp = w_bmp;
            _h_bmp = h_bmp;
            _margins = new float[] { 20, 50, 20, 40 };
            _max_x = 0;
            _min = 0;
            _max = 0;
            _title = "";
            _abscissaReferences = new List<double>();
            _maximumValues = new List<double[]>();
            _curves = new List<Curve>();
            _referenceOrdinates = new Dictionary<double, Color>();
            _referencePoints = new Dictionary<PointF, Color>();
            _watermark = null;
            _drawLegend = true;
        }

        /// <summary>
        /// Add the minimum/maximum values to draw the envelope curve
        /// </summary>
        /// <param name="x">The x value</param>
        /// <param name="min">The min value vor the given x</param>
        /// <param name="max">The max value vor the given x</param>
        public void AddMaximumValues(double x, double min, double max)
        {
            if (_maximumValues.Where(v => v[0] == x).Count() > 0)
                return;
            double[] vals = new double[] { x, min, max };
            _maximumValues.Add(vals);
            if (min < _min)
                _min = min;
            if (max > _max)
                _max = max;
            if (x > _max_x)
                _max_x = x;
        }

        /// <summary>
        /// Add a new curve in the graphics
        /// </summary>
        /// <param name="name">The name of the curve</param>
        /// <param name="color">The color of the curve</param>
        /// <returns></returns>
        public bool AddCurve(string name, Color color)
        {
            if (_curves.SingleOrDefault(c => c.Name == name) != null)
                return false;
            _curves.Add(new Curve(name, color));
            return true;
        }

        /// <summary>
        /// Add a new value to an existing curve defined with the AddCurve() function
        /// </summary>
        /// <param name="name">The name of the curve</param>
        /// <param name="x">The x value in the graph</param>
        /// <param name="y">The y value in the graph</param>
        /// <returns></returns>
        public bool AddCurveValue(string name, double x, double y)
        {
            Curve curve = _curves.SingleOrDefault(c => c.Name == name);
            if (curve == null)
                return false;

            if (curve.Values.Where(v => v[0] == x).Count() == 0)
            {
                double[] vals = new double[] { x, y };
                curve.Values.Add(vals);

                if (y < _min)
                    _min = y;
                if (y > _max)
                    _max = y;
                if (x > _max_x)
                    _max_x = x;
            }

            return true;
        }

        /// <summary>
        /// Add an horizontal reference line at the given value in thhe ordinate axis
        /// </summary>
        /// <param name="value">The reference value</param>
        /// <param name="color">The color of the line</param>
        /// <returns></returns>
        public bool AddOrdinateReference(double value, Color color)
        {
            try
            {
                _referenceOrdinates.Add(value, color);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool AddReferencePoint(double x, double y, Color color)
        {
            try
            {
                _referencePoints.Add(new PointF((float)x, (float)y), color);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create the graphic bitmap
        /// </summary>
        /// <param name="showMaxMin">Show the minumum and the maximun values</param>
        /// <param name="scale">The scale factor in abscissa direction</param>
        /// <returns></returns>
        public Bitmap Create(bool showMaxMin, float scale = 1)
        {
            Bitmap image = new Bitmap((int)_w_bmp, (int)_h_bmp);

            try
            {
                // Local copy: the margins are enlarged for the legend and the axis labels, Create() can be called more than once
                float[] margins = (float[])_margins.Clone();

                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image))
                using (Font axisFont = new Font("Arial", 10))
                {
                    // Draw the background
                    g.FillRectangle(Brushes.White, 0, 0, _w_bmp - 1, _h_bmp - 1);
                    g.DrawRectangle(Pens.Black, 0, 0, _w_bmp - 1, _h_bmp - 1);
                    using (Font font = new Font("Arial", 15))
                    using (StringFormat format = new StringFormat())
                    {
                        format.Alignment = StringAlignment.Center;
                        g.DrawString(_title, font, Brushes.Black, new PointF(image.Width / 2, 5), format);
                    }

                    // Legend
                    if (_drawLegend && _curves.Count > 0)
                    {
                        using (Font font = new Font("Arial", 10))
                        using (StringFormat format = new StringFormat())
                        {
                            float w_legend = _curves.Max(c => g.MeasureString(c.Name, font).Width) + 5;
                            float h_legend = g.MeasureString("ABC", font).Height;

                            g.DrawRectangle(Pens.Black, _w_bmp - margins[2] - 5 - w_legend, margins[1] - 5, w_legend, (h_legend + 1) * _curves.Count + 5);

                            format.Alignment = StringAlignment.Near;
                            format.LineAlignment = StringAlignment.Near;
                            float y = margins[1];
                            foreach (Curve curve in _curves)
                            {
                                using (Brush brush = new SolidBrush(curve.Color))
                                    g.DrawString(curve.Name, font, brush, new PointF(_w_bmp - margins[2] - w_legend, y), format);
                                y += h_legend + 1;
                            }
                            margins[2] += w_legend + 10;
                        }
                    }

                    // Calculate margin for numeric values of y axis
                    margins[0] += Math.Max(g.MeasureString(_min.ToString("N0"), axisFont).Width, g.MeasureString(_max.ToString("N0"), axisFont).Width);

                    // Define logical viewport (a null height would make the transformation singular)
                    double yRange = _max - _min;
                    if (yRange <= 0)
                        yRange = 1.0;
                    double xRange = _max_x > 0 ? _max_x : 1.0;
                    RectangleF srcRect = new RectangleF((float)0, (float)_min, (float)xRange, (float)yRange);
                    RectangleF dstRect = new RectangleF(margins[0], margins[1], _w_bmp - margins[0] - margins[2], _h_bmp - margins[1] - margins[3]);

                    System.Drawing.Drawing2D.GraphicsContainer container = g.BeginContainer(dstRect, srcRect, GraphicsUnit.Millimeter);

                    g.ScaleTransform(1.0F, -1.0F);
                    g.TranslateTransform(0.0F, -(float)(_min * 2) - srcRect.Height);
                    g.PageUnit = GraphicsUnit.Millimeter;

                    PointF[] limits_crvs_pts = new PointF[2];
                    double[] limits_crvs_val = new double[2];
                    bool hasCurvesLimits = false;

                    PointF[] limits_maxs_pts = new PointF[2];
                    double[] limits_maxs_val = new double[2];
                    bool hasMaximumLimits = false;

                    if (showMaxMin)
                    {
                        for (int n = 0; n < _maximumValues.Count - 1; n++)
                        {
                            PointF[] points_max = new PointF[4];
                            points_max[0] = new PointF((float)_maximumValues[n][0], 0);
                            points_max[1] = new PointF((float)_maximumValues[n][0], (float)_maximumValues[n][2] * scale);
                            points_max[2] = new PointF((float)_maximumValues[n + 1][0], (float)_maximumValues[n + 1][2] * scale);
                            points_max[3] = new PointF((float)_maximumValues[n + 1][0], 0);
                            g.FillPolygon(Brushes.LightGray, points_max);
                            if (points_max[1].Y >= 0 && points_max[2].Y >= 0)
                                g.DrawLine(Pens.Gray, points_max[1], points_max[2]);

                            PointF[] points_min = new PointF[4];
                            points_min[0] = new PointF((float)_maximumValues[n][0], 0);
                            points_min[1] = new PointF((float)_maximumValues[n][0], (float)_maximumValues[n][1] * scale);
                            points_min[2] = new PointF((float)_maximumValues[n + 1][0], (float)_maximumValues[n + 1][1] * scale);
                            points_min[3] = new PointF((float)_maximumValues[n + 1][0], 0);
                            g.FillPolygon(Brushes.LightGray, points_min);
                            if (points_min[1].Y <= 0 && points_min[2].Y <= 0)
                                g.DrawLine(Pens.Gray, points_min[1], points_min[2]);

                            if (!hasMaximumLimits)
                            {
                                limits_maxs_pts[0] = points_min[1];
                                limits_maxs_pts[1] = points_max[1];
                                limits_maxs_val[0] = _maximumValues[n][1];
                                limits_maxs_val[1] = _maximumValues[n][2];
                                hasMaximumLimits = true;
                            }

                            if (points_min[1].Y < limits_maxs_pts[0].Y)
                            {
                                limits_maxs_pts[0] = points_min[1];
                                limits_maxs_val[0] = _maximumValues[n][1];
                            }
                            if (points_min[2].Y < limits_maxs_pts[0].Y)
                            {
                                limits_maxs_pts[0] = points_min[2];
                                limits_maxs_val[0] = _maximumValues[n + 1][1];
                            }
                            if (points_max[1].Y > limits_maxs_pts[1].Y)
                            {
                                limits_maxs_pts[1] = points_max[1];
                                limits_maxs_val[1] = _maximumValues[n][2];
                            }
                            if (points_max[2].Y > limits_maxs_pts[1].Y)
                            {
                                limits_maxs_pts[1] = points_max[2];
                                limits_maxs_val[1] = _maximumValues[n + 1][2];
                            }
                        }
                    }
                    foreach (Curve curve in _curves)
                    {
                        using (Pen pen = new Pen(curve.Color, 0.1F))
                        for (int n = 0; n < curve.Values.Count - 1; n++)
                        {
                            PointF start = new PointF((float)curve.Values[n][0], (float)curve.Values[n][1] * scale);
                            PointF end = new PointF((float)curve.Values[n + 1][0], (float)curve.Values[n + 1][1] * scale);
                            if (!hasCurvesLimits)
                            {
                                limits_crvs_pts[0] = start;
                                limits_crvs_pts[1] = start;
                                limits_crvs_val[0] = curve.Values[n][1];
                                limits_crvs_val[1] = curve.Values[n][1];
                                hasCurvesLimits = true;
                            }

                            if (start.Y < limits_crvs_pts[0].Y)
                            {
                                limits_crvs_pts[0] = start;
                                limits_crvs_val[0] = curve.Values[n][1];
                            }
                            if (end.Y < limits_crvs_pts[0].Y)
                            {
                                limits_crvs_pts[0] = end;
                                limits_crvs_val[0] = curve.Values[n + 1][1];
                            }
                            if (start.Y > limits_crvs_pts[1].Y)
                            {
                                limits_crvs_pts[1] = start;
                                limits_crvs_val[1] = curve.Values[n][1];
                            }
                            if (end.Y > limits_crvs_pts[1].Y)
                            {
                                limits_crvs_pts[1] = end;
                                limits_crvs_val[1] = curve.Values[n + 1][1];
                            }

                            g.DrawLine(pen, start, end);
                        }
                    }

                    // Points for drawing axies
                    PointF[] x_axis_pts = new PointF[] { new PointF(0F, 0F), new PointF((float)_max_x, 0F) };
                    g.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Device, System.Drawing.Drawing2D.CoordinateSpace.World, x_axis_pts);
                    PointF[] y_axis_pts = new PointF[] { new PointF(0F, (float)_max), new PointF(0F, (float)_min) };
                    g.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Device, System.Drawing.Drawing2D.CoordinateSpace.World, y_axis_pts);

                    List<PointF> x_refs_list = new List<PointF>();
                    foreach (double len in _abscissaReferences)
                        x_refs_list.Add(new PointF((x_refs_list.Count > 0 ? x_refs_list.Last().X : 0) + (float)len, 0));
                    PointF[] x_refs_pts = x_refs_list.ToArray();
                    if (x_refs_pts.Length > 0)
                        g.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Device, System.Drawing.Drawing2D.CoordinateSpace.World, x_refs_pts);

                    double step = GetAxisStep(Math.Max(Math.Abs(_min), _max) / 5.0);
                    List<PointF> y_refs_list = new List<PointF>();
                    List<string> y_values = new List<string>();
                    for (double y = 0; y < _max; y += step)
                    {
                        y_refs_list.Add(new PointF(0, (float)y * scale));
                        y_values.Add(y.ToString("G6"));

                        if (y_values.Count > 200) //exit if too much iteration
                        {
                            y = _max + 1;
                        }
                    }
                    for (double y = -step; y > _min; y -= step) // zero already added by the previous loop
                    {
                        y_refs_list.Add(new PointF(0, (float)y * scale));
                        y_values.Add(y.ToString("G6"));

                        if (y_values.Count > 200) //exit if too much iteration
                        {
                            y = _min - 1;
                        }
                    }

                    PointF[] y_refs_pts = y_refs_list.ToArray();
                    if (y_refs_pts.Length > 0)
                    {
                        g.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Device, System.Drawing.Drawing2D.CoordinateSpace.World, y_refs_pts);
                        g.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Device, System.Drawing.Drawing2D.CoordinateSpace.World, limits_crvs_pts);
                        g.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Device, System.Drawing.Drawing2D.CoordinateSpace.World, limits_maxs_pts);
                    }

                    // Saves the reference lines y coords
                    PointF[] ref_line_start = new PointF[_referenceOrdinates.Count];
                    int r = 0;
                    foreach (KeyValuePair<double, Color> ref_line in _referenceOrdinates)
                        ref_line_start[r++] = new PointF(0, (float)ref_line.Key * scale);
                    if (_referenceOrdinates.Count > 0)
                        g.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Device, System.Drawing.Drawing2D.CoordinateSpace.World, ref_line_start);


                    // Saves the reference points coordinates
                    PointF[] ref_points = new PointF[_referencePoints.Count];
                    r = 0;
                    foreach (KeyValuePair<PointF, Color> ref_point in _referencePoints)
                        ref_points[r++] = new PointF((float)ref_point.Key.X, (float)ref_point.Key.Y * scale);
                    if (_referencePoints.Count > 0)
                        g.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Device, System.Drawing.Drawing2D.CoordinateSpace.World, ref_points);


                    g.EndContainer(container);

                    // Draw the X axis
                    g.DrawLine(Pens.Black, x_axis_pts[0], x_axis_pts[1]);
                    if (x_refs_pts.Length > 0)
                    {
                        for (int i = 0; i < x_refs_pts.Length; i++)
                            g.DrawLine(Pens.Black, x_refs_pts[i].X, x_refs_pts[i].Y - 5, x_refs_pts[i].X, x_refs_pts[i].Y + 5);
                    }

                    // Draw the Y axis
                    g.DrawLine(Pens.Black, y_axis_pts[0], y_axis_pts[1]);
                    if (y_refs_pts.Length > 0)
                    {
                        using (StringFormat format_axis = new StringFormat())
                        {
                            format_axis.Alignment = StringAlignment.Far;
                            format_axis.LineAlignment = StringAlignment.Center;
                            for (int i = 0; i < y_refs_pts.Length; i++)
                            {
                                g.DrawLine(Pens.Black, y_refs_pts[i].X - 5, y_refs_pts[i].Y, y_refs_pts[i].X + 5, y_refs_pts[i].Y);
                                g.DrawString(y_values[i], axisFont, Brushes.Black, y_refs_pts[i].X - 6, y_refs_pts[i].Y, format_axis);
                            }
                            format_axis.Alignment = StringAlignment.Center;
                            format_axis.LineAlignment = StringAlignment.Near;
                            if (hasCurvesLimits)
                                g.DrawString(limits_crvs_val[0].ToString("N2"), axisFont, Brushes.Black, limits_crvs_pts[0], format_axis);
                            if (showMaxMin && hasMaximumLimits && (!hasCurvesLimits || limits_maxs_val[0] != limits_crvs_val[0]))
                                g.DrawString(limits_maxs_val[0].ToString("N2"), axisFont, Brushes.Black, limits_maxs_pts[0], format_axis);

                            format_axis.LineAlignment = StringAlignment.Far;
                            if (hasCurvesLimits)
                                g.DrawString(limits_crvs_val[1].ToString("N2"), axisFont, Brushes.Black, limits_crvs_pts[1], format_axis);
                            if (showMaxMin && hasMaximumLimits && (!hasCurvesLimits || limits_maxs_val[1] != limits_crvs_val[1]))
                                g.DrawString(limits_maxs_val[1].ToString("N2"), axisFont, Brushes.Black, limits_maxs_pts[1], format_axis);
                        }
                    }

                    // Draw reference lines
                    r = 0;
                    foreach (KeyValuePair<double, Color> ref_line in _referenceOrdinates)
                    {
                        using (Pen pen = new Pen(ref_line.Value))
                            g.DrawLine(pen, ref_line_start[r], new PointF(x_axis_pts[1].X, ref_line_start[r].Y));
                        using (StringFormat format_refline = new StringFormat())
                        using (Brush brush = new SolidBrush(ref_line.Value))
                        {
                            format_refline.Alignment = StringAlignment.Near;
                            format_refline.LineAlignment = ref_line.Key > 0 ? StringAlignment.Far : StringAlignment.Near;
                            g.DrawString(ref_line.Key.ToString("N2"), axisFont, brush, ref_line_start[r], format_refline);
                        }
                        r++;
                    }

                    // Draw reference points
                    r = 0;
                    foreach (KeyValuePair<PointF, Color> ref_point in _referencePoints)
                    {
                        using (Brush brush = new SolidBrush(ref_point.Value))
                            g.FillEllipse(brush, new RectangleF(ref_points[r].X - 4.0f, ref_points[r].Y - 4.0f, 8.0f, 8.0f));
                        r++;
                    }

                    DrawWatermark(g);
                }
            }
            catch (Exception /*e*/)
            {
                //Xceed.Wpf.Toolkit.MessageBox.Show(App.Current.MainWindow, e.Message, "GraphicCreator class", System.Windows.MessageBoxButton.OK,
                //    System.Windows.MessageBoxImage.Error);
                image.Dispose();
                return null;
            }

            return image;
        }

        /// <returns>A "round" step (1, 2 or 5 times a power of ten) greater or equal to <paramref name="rawStep"/>. 1 if <paramref name="rawStep"/> is not positive</returns>
        private static double GetAxisStep(double rawStep)
        {
            if (!(rawStep > 0) || double.IsInfinity(rawStep))
                return 1.0;

            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
            double normalized = rawStep / magnitude;

            if (normalized <= 1.0)
                return magnitude;
            if (normalized <= 2.0)
                return 2.0 * magnitude;
            if (normalized <= 5.0)
                return 5.0 * magnitude;
            return 10.0 * magnitude;
        }

        private void DrawWatermark(System.Drawing.Graphics g)
        {
            if (_watermark != null)
            {
                float x = (_w_bmp - _watermark.Width) / 2;
                float y = (_h_bmp - _watermark.Height) / 2;
                g.DrawImage(_watermark, x, y);
            }
        }

        public void Dispose()
        {
            if (_watermark != null)
                _watermark.Dispose();
        }
    }
}
