
using System;
using System.Collections.Generic;
using System.Linq;
using Svg.Transforms;
using SkiaSharp;

namespace Svg.SkiaSharp
{
    // maybe copy from http://stackoverflow.com/questions/15817888/fast-rotation-transformation-matrix-multiplications ?
    // see also exmplanations at https://www.willamette.edu/~gorr/classes/GeneralGraphics/Transforms/transforms2d.htm
    public class Matrix : IDisposable
    {
        public override string ToString()
        {
            var e = Elements;
            return $"[{e[0]};{e[1]};{e[2]}],[{e[3]};{e[4]};{e[5]}],[{e[6]};{e[7]};{e[8]}]";
        }
        public override bool Equals(object obj)
        {
            var matrix = obj as Matrix;
            if (matrix == null)
                return false;

            return Elements.SequenceEqual(matrix.Elements);
        }

        public float RotationDegrees
        {
            get { return (float)RadianToDegree(Math.Atan(SkewY/ScaleY)); }
        }

        public float Rotation
        {
            get { return (float)(Math.Atan(SkewY / ScaleY)); }
        }

        /// <summary>
        /// Multiplies matriy a with b like "a*b"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public float[] Multiply(float[] a, float[] b)
        {
            //var a = new float[9];
            //var b = new float[9];
            //_m.GetValues(a);
            //other._m.GetValues(b);

            var a1 = new float[3, 3];
            a1[0, 0] = a[0];
            a1[0, 1] = a[1];
            a1[0, 2] = a[2];
            a1[1, 0] = a[3];
            a1[1, 1] = a[4];
            a1[1, 2] = a[5];
            a1[2, 0] = a[6];
            a1[2, 1] = a[7];
            a1[2, 2] = a[8];

            var b1 = new float[3, 3];
            b1[0, 0] = b[0];
            b1[0, 1] = b[1];
            b1[0, 2] = b[2];
            b1[1, 0] = b[3];
            b1[1, 1] = b[4];
            b1[1, 2] = b[5];
            b1[2, 0] = b[6];
            b1[2, 1] = b[7];
            b1[2, 2] = b[8];


            var r = MultiplyMatrix(a1, b1);
            var result = new float[]
            {
                r[0, 0], r[0, 1], r[0, 2],
                r[1, 0], r[1, 1], r[1, 2],
                r[2, 0], r[2, 1], r[2, 2],
            };
            return result;
            //_m.SetValues();
        }

        /// <summary>
        /// Multiplies matriy a with b like "a*b"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public float[,] MultiplyMatrix(float[,] a, float[,] b)
        {
            int rA = a.GetLength(0);
            int cA = a.GetLength(1);
            int rB = b.GetLength(0);
            int cB = b.GetLength(1);
            float temp = 0;
            float[,] kHasil = new float[rA, cB];
            if (cA != rB)
            {
                throw new InvalidOperationException("matrix cannot be multiplied");
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += a[i, k] * b[k, j];
                        }
                        kHasil[i, j] = temp;
                    }
                }
                return kHasil;
            }
        }

         public SvgMatrix ToSvgMatrix()
        {
            var points = new List<float>
            {
                ScaleX,
                SkewY, // x and y need to be swapped!
                SkewX, // x and y need to be swapped!
                ScaleY,
                OffsetX,
                OffsetY
            };
            return new SvgMatrix(points);
        }

        public static implicit operator SvgMatrix(Matrix other)
        {
            return other.ToSvgMatrix();
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }


        private SKMatrix _m;

        /// <summary>
        /// Creates a new instance of <see cref="Matrix"/>, the initial value is an identity matrix.
        /// </summary>
        public Matrix()
        {
            _m = SKMatrix.MakeIdentity();
        }

        public Matrix(SKMatrix src)
        {
            _m = new SKMatrix();
            _m.Persp0 = src.Persp0;
            _m.Persp1 = src.Persp1;
            _m.Persp2 = src.Persp2;
            _m.ScaleX = src.ScaleX;
            _m.ScaleY = src.ScaleY;
            _m.SkewX = src.SkewX;
            _m.SkewY = src.SkewY;
            _m.TransX = src.TransX;
            _m.TransY = src.TransY;
        }

        public Matrix(SKMatrix src, bool copy)
        {
            _m = src;
        }

        public Matrix(float[] e)
        {
            _m = new SKMatrix()
            {
                ScaleX = e[0],
                SkewX = e[1],
                TransX = e[2],
                SkewY = e[3],
                ScaleY = e[4],
                TransY = e[5],
                Persp0 = e[6],
                Persp1 = e[7],
                Persp2 = e[8]
            };
        }

        //
        // Summary:
        //     Initializes a new instance of the Matrix class with
        //     the specified elements.
        //
        // see: https://msdn.microsoft.com/en-us/library/system.drawing.drawing2d.matrix(v=vs.110).aspx
        public Matrix(float scaleX, float rotateX, float rotateY, float scaleY, float transX, float transY)
        {
            _m = new SKMatrix();

            /*
             * In android, rotateX and rotateY are switched for whatever reason!!
             */
            _m.ScaleX = scaleX;
            _m.SkewX = rotateY;
            _m.TransX = transX;
            _m.SkewY = rotateX;
            _m.ScaleY = scaleY;
            _m.TransY = transY;
            _m.Persp0 = 0;
            _m.Persp1 = 0;
            _m.Persp2 = 1;

            /*      see:https://github.com/google/skia/blob/master/src/core/SkMatrix.cpp
             *      [scale-x    skew-x      trans-x]   [X]   [X']
             *      [skew-y     scale-y     trans-y] * [Y] = [Y']
             *      [persp-0    persp-1     persp-2]   [1]   [1 ]
            */

        }

        public SKMatrix SKMatrix => _m;

        public  bool IsIdentity
        {
            get
            {
                return _m.ScaleX == 1f && _m.SkewX == 0f && _m.TransX == 0f &&
                       _m.SkewY == 0f && _m.ScaleY == 1f && _m.TransY == 0f &&
                       _m.Persp0 == 0f && _m.Persp1 == 0f && _m.Persp2 == 1f;
            }
        }

        public  void Invert()
        {
            //// copied from SkMatrix::invertNonIdentity
            //// see: https://github.com/google/skia/blob/master/src/core/SkMatrix.cpp
            //if (IsIdentity)
            //    return;

            //var m = _m;

            //bool isScaleMatrix = m.ScaleX != 1f || m.ScaleY != 1f;
            //bool isTranslateMatrix = m.TransX != 0f || m.TransY != 0f;
            //bool isRotateMatrix = m.SkewX != 0f || m.SkewX != 0f;

            //if (!isRotateMatrix && isScaleMatrix && isTranslateMatrix)
            //{
            //    var invX = m.ScaleX;
            //    var invY = m.ScaleY;
            //    if (invX == 0 || invY == 0)
            //    {
            //        // not invertible
            //        return;
            //    }
            //    invX = 1/invX;
            //    invY = 1/invY;
            //    m.SkewX = 0;
            //    m.SkewY = 0;
            //    m.Persp0 = 0;
            //    m.Persp1 = 0;

            //    m.ScaleX = invX;
            //    m.ScaleY = invY;
            //    m.Persp2 = 1;
            //    m.TransX = -m.TransX*invX;
            //    m.TransY = -m.TransY*invY;

            //    _m = m;
            //    return;
            //}
            //else if (!isRotateMatrix && isTranslateMatrix)
            //{
            //    m.TransX = -m.TransX;
            //    m.TransY = -m.TransY;
            //    _m = m;
            //    return;
            //}

            //var m = _m;
            //float det = m.ScaleX * (m.ScaleY * m.Persp2 - m.Persp1 * m.TransY) -
            // m.SkewX * (m.SkewY * m.Persp2 - m.TransY * m.Persp0) +
            // m.TransX * (m.SkewY * m.Persp1 - m.ScaleY * m.Persp0);

            //float invdet = 1 / det;

            //var m1 = new SKMatrix();
            //m1.ScaleX = (m.ScaleY * m.Persp2 - m.Persp1 * m.TransY) * invdet;
            //m1.SkewX = (m.TransX * m.Persp1 - m.SkewX * m.Persp2) * invdet;
            //m1.TransX = (m.SkewX * m.TransY - m.TransX * m.ScaleY) * invdet;
            //m1.SkewY = (m.TransY * m.Persp0 - m.SkewY * m.Persp2) * invdet;
            //m1.ScaleY = (m.ScaleX * m.Persp2 - m.TransX * m.Persp0) * invdet;
            //m1.TransY = (m.SkewY * m.TransX - m.ScaleX * m.TransY) * invdet;
            //m1.Persp0 = (m.SkewY * m.Persp1 - m.Persp0 * m.ScaleY) * invdet;
            //m1.Persp1 = (m.Persp0 * m.SkewX - m.ScaleX * m.Persp1) * invdet;
            //m1.Persp2 = (m.ScaleX * m.ScaleY - m.SkewY * m.SkewX) * invdet;
            //_m = m1;

            SKMatrix m;
            if (_m.TryInvert(out m))
            {
                _m = m;
            }
        }

        public  void Scale(float width, float height)
        {
            Scale(width, height, MatrixOrder.Prepend);
        }

        public  void Scale(float width, float height, MatrixOrder order)
        {
            var m = SKMatrix.MakeScale(width, height);

            if (order == MatrixOrder.Append)
                SKMatrix.PostConcat(ref _m, ref m);
            else
                SKMatrix.PreConcat(ref _m, ref m);
        }

        public void Translate(float left, float top)
        {
            Translate(left, top, MatrixOrder.Prepend);
        }

        public void Translate(float left, float top, MatrixOrder order)
        {
            var m = SKMatrix.MakeTranslation(left, top);

            if (order == MatrixOrder.Append)
                SKMatrix.PostConcat(ref _m, ref m);
            else
                SKMatrix.PreConcat(ref _m, ref m);
        }

        /// <summary>
        /// Does a pre-pend multiplication
        /// </summary>
        /// <param name="matrix"></param>
        public void Multiply(Matrix matrix)
        {
            Multiply(matrix, MatrixOrder.Prepend);
        }

        public void Multiply(Matrix matrix, MatrixOrder order)
        {
            var m = ((Matrix)matrix).SKMatrix;

            if (order == MatrixOrder.Append)
                SKMatrix.PostConcat(ref _m, ref m);
            else
                SKMatrix.PreConcat(ref _m, ref m);
        }

        public void Rotate(float angleDegrees, MatrixOrder order)
        {
            var m = SKMatrix.MakeRotationDegrees(angleDegrees);

            if (order == MatrixOrder.Append)
                SKMatrix.PostConcat(ref _m, ref m);
            else
                SKMatrix.PreConcat(ref _m, ref m);
        }

        public void RotateAt(float angleDegrees, PointF midPoint, MatrixOrder order)
        {
            var m = SKMatrix.MakeRotationDegrees(angleDegrees, midPoint.X, midPoint.Y);

            if (order == MatrixOrder.Append)
                SKMatrix.PostConcat(ref _m, ref m);
            else
                SKMatrix.PreConcat(ref _m, ref m);
        }

        public void Rotate(float angleDegrees)
        {
            Rotate(angleDegrees, MatrixOrder.Prepend);
        }

        public void Shear(float sx, float sy)
        {
            var m = SKMatrix.MakeSkew(sx, sy);

            SKMatrix.PreConcat(ref _m, ref m);
        }

        public RectangleF TransformRectangle(RectangleF b)
        {
            var p1 = new PointF(b.Left, b.Top);
            var p2 = new PointF(b.Right, b.Top);
            var p3 = new PointF(b.Right, b.Bottom);
            var p4 = new PointF(b.Left, b.Bottom);
            var pts = new[] { p1, p2, p3, p4 };

            TransformPoints(pts);

            return RectangleF.FromPoints(pts);
        }

        public void TransformVectors(PointF[] points)
        {
            var pts = points.Select(p => new SKPoint(p.X, p.Y)).ToArray();

            var mappedPoints = _m.MapVectors(pts);
            for (int i = 0; i < mappedPoints.Length; i++)
            {
                points[i].X = mappedPoints[i].X;
                points[i].Y = mappedPoints[i].Y;
            }
        }

        public void TransformPoints(PointF[] points)
        {
            var pts = points.Select(p => new SKPoint(p.X, p.Y)).ToArray();

            var mappedPoints = _m.MapPoints(pts);
            for (int i = 0; i < mappedPoints.Length; i++)
            {
                points[i].X = mappedPoints[i].X;
                points[i].Y = mappedPoints[i].Y;
            }
        }

        public float[] Elements
        {
            get
            {
                var res = new float[9]
                {
                    _m.ScaleX,
                    _m.SkewX,
                    _m.TransX,
                    _m.SkewY,
                    _m.ScaleY,
                    _m.TransY,
                    _m.Persp0,
                    _m.Persp1,
                    _m.Persp2,
                };
                return res;
            }
        }

        public float OffsetX
        {
            get { return _m.TransX; }
        }

        public float OffsetY
        {
            get { return _m.TransY; }
        }

        public float ScaleX
        {
            get { return _m.ScaleX; }
        }

        public  float ScaleY
        {
            get { return _m.ScaleY; }
        }

        public  float SkewX
        {
            get { return _m.SkewX; }
        }

        public  float SkewY
        {
            get { return _m.SkewY; }
        }

        private static float[] GetElements(SKMatrix m)
        {
            return new float[9]
                {
                    m.ScaleX,
                    m.SkewX,
                    m.TransX,
                    m.SkewY,
                    m.ScaleY,
                    m.TransY,
                    m.Persp0,
                    m.Persp1,
                    m.Persp2,
                };
        }

        public static implicit operator Matrix(SKMatrix other)
        {
            return new Matrix(other, true);
        }

        public static implicit operator SKMatrix(Matrix other)
        {
            return other.SKMatrix;
        }

        public  Matrix Clone()
        {
            return new Matrix(_m);
        }

        public void Dispose()
        {
        }
    }
}