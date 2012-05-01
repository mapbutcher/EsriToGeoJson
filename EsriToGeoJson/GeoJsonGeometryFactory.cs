#region File Information
//
// File: ""
// Purpose: ""
// Author: "Geoplex"
// 
#endregion

#region (c) Copyright "2011" Geoplex
//
// This is UNPUBLISHED PROPRIETARY SOURCE CODE of GeoPlex.
// The contents of this file may not be disclosed to third parties, copied or
// duplicated in any form, in whole or in part, without the prior written
// permission of GeoPlex.
//
// THE SOFTWARE IS PROVIDED "AS-IS" AND WITHOUT WARRANTY OF ANY KIND,
// EXPRESS, IMPLIED OR OTHERWISE, INCLUDING WITHOUT LIMITATION, ANY
// WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
//
// IN NO EVENT SHALL GEOPLEX BE LIABLE FOR ANY SPECIAL, INCIDENTAL,
// INDIRECT OR CONSEQUENTIAL DAMAGES OF ANY KIND, OR ANY DAMAGES WHATSOEVER
// RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER OR NOT ADVISED OF THE
// POSSIBILITY OF DAMAGE, AND ON ANY THEORY OF LIABILITY, ARISING OUT OF OR IN
// CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
//
#endregion

using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geometry;
using GeoJSON.Net.Geometry;

namespace EsriToGeoJson
{
    /// <summary>
    /// 
    /// </summary>
    class GeoJsonGeometryFactory
    {
        #region Member Variables
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Public Methods

        public IGeometryObject GetGeometry(IGeometry esriGeometry)
        {
            IGeometryObject geoJsonGeom = null;

            string error = string.Format("A Geometry of type {0} cannot be created", Enum.GetName(typeof(esriGeometryType), esriGeometry.GeometryType));

            switch (esriGeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryAny:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryBag:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryBezier3Curve:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryCircularArc:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryEllipticArc:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryEnvelope:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryLine:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryMultiPatch:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryMultipoint:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryNull:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryPath:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryPoint:
                    //cast to esri obj
                    IPoint point = esriGeometry as IPoint;

                    List<double> coords = new List<double> { point.X, point.Y };

                    //create geojson obj
                    GeoJSON.Net.Geometry.Point pnt = new GeoJSON.Net.Geometry.Point(coords);
                    geoJsonGeom = pnt;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    IGeometryCollection polygons = esriGeometry as IGeometryCollection;

                    List<LineString> linearRings = new List<LineString>();

                    var rings = new List<List<List<double>>>();
                    var ring = new List<List<double>>();

                    //todo - how does esri handle polygons with holes
                    for (int i = 0; i < polygons.GeometryCount; i++)
                    {
                        //for each geometry in the polygon create a point collection
                        var pGeom = polygons.Geometry[i];
                        var pntCollection = (IPointCollection)pGeom;

                        //GeoJSON.Net.Geometry.LineString line = new LineString();
                        for (var a = 0; a < pntCollection.PointCount; a++)
                        {
                            //create a list of coordinates for each line
                            List<double> linePointCoords = new List<double> { pntCollection.Point[a].X, pntCollection.Point[a].Y };

                            //..and add them to the poly
                            ring.Add(linePointCoords);
                        }

                        rings.Add(ring);
                    }

                    //initialise a poly
                    GeoJSON.Net.Geometry.Polygon poly = new GeoJSON.Net.Geometry.Polygon(rings);

                    geoJsonGeom = poly;
                    break;
                case esriGeometryType.esriGeometryPolyline:

                    //cast to esri obj
                    IGeometryCollection polylines = esriGeometry as IGeometryCollection;

                    //create geojson obj
                    GeoJSON.Net.Geometry.LineString polyLine = new LineString();


                    for (int i = 0; i < polylines.GeometryCount; i++)
                    {
                        //for each geometry in the polyline create a point collection
                        var pGeom = polylines.Geometry[i];
                        var pntCollection = (IPointCollection)pGeom;


                        for (var a = 0; a < pntCollection.PointCount; a++)
                        {
                            //create a list of coordinates for each line
                            List<double> linePointCoords = new List<double> { pntCollection.Point[a].X, pntCollection.Point[a].Y };

                            //..and add them to the line
                            polyLine.Coordinates.Add(linePointCoords);
                        }
                    }

                    geoJsonGeom = polyLine;

                    break;
                case esriGeometryType.esriGeometryRay:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryRing:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometrySphere:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryTriangleFan:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryTriangleStrip:
                    throw new ArgumentOutOfRangeException(error);
                case esriGeometryType.esriGeometryTriangles:
                    throw new ArgumentOutOfRangeException(error);
                default:
                    throw new ArgumentOutOfRangeException(error);
            }

            return geoJsonGeom;

        }


        #endregion

        #region Private Methods
        #endregion

        #region Enums
        #endregion
    }
}
