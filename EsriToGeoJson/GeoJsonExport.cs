#region File Information
//
// File: "GeoJsonExport"
// Purpose: "Provide a GeoJson representation of esri objects"
// Author: "Geoplex"
// 
#endregion

#region (c) Copyright "2012" Geoplex
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
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;


namespace EsriToGeoJson
{
    /// <summary>
    /// 
    /// </summary>
    public class GeoJsonExport
    {
        #region Member Variables
        #endregion

        #region Constructors

        public GeoJsonExport()
        {

        }

        #endregion

        #region Properties

        private string _geoJson;

        public string GeoJson
        {
            get { return _geoJson; }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the geojson export.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="exportProperties">The export properties.</param>
        public void CreateExport(IRecordSet results, string geometryFieldName)
        {
            if (results != null)
            {
                ICursor cursor;
                cursor = results.get_Cursor(false);
                IRow row = cursor.NextRow();
                int rowCount = 0;


                //create an empty list of geojson features
                var geoJsonFeatures = new List<GeoJSON.Net.Feature.Feature>();

                //get the index of the geometry field
                int fieldIndex = row.Fields.FindField(geometryFieldName);
                if (fieldIndex == -1)
                    throw new Exception("Could not locate geometry field:shape");

                if (row != null)
                {

                    while (row != null)
                    {

                        //use the factory to convert the esri geometry to the geojson geometry
                        IGeometryObject geoJsonGeom = null;
                        if (row.get_Value(fieldIndex) != null)
                        {
                            GeoJsonGeometryFactory geomFactory = new GeoJsonGeometryFactory();
                            geoJsonGeom = geomFactory.GetGeometry(row.get_Value(fieldIndex) as IGeometry);
                        }

                        //use the factory to convert esri row to geojson attributes
                        var atts = new GeoJsonAttributeFactory().GetAttributes(row);


                        //create the feature and add it to the collection - use the unique key from the row or create one
                        GeoJSON.Net.Feature.Feature f = null;
                        if (row.Table.HasOID)
                        {
                            f = new GeoJSON.Net.Feature.Feature(geoJsonGeom, atts) { Id = row.get_Value(row.Fields.FindField(row.Table.OIDFieldName)).ToString() };
                        }
                        else
                        {
                            f = new GeoJSON.Net.Feature.Feature(geoJsonGeom, atts) { Id = Guid.NewGuid().ToString() };
                        }

                        geoJsonFeatures.Add(f);

                        row = cursor.NextRow();

                        rowCount++;
                    }
                }

                //initialise the geoJsonFeatureCollection
                var fc = new FeatureCollection(geoJsonFeatures);

                //to keep the export small, use formatting = none and exclude null values
                //todo - allow the null value handling and indentation to be exposed
                _geoJson = JsonConvert.SerializeObject(fc, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            }

        }

        /// <summary>
        /// Creates the geojson export.
        /// </summary>
        /// <param name="featureClass">The feature class.</param>
        /// <param name="exportProperties">The export properties.</param>
        public void CreateExport(IFeatureClass featureClass, string exportProperties)
        {
            CreateExport(ConvertToRecordset(featureClass), exportProperties);
        }

        #endregion

        #region Private Methods


        /// <summary>
        /// Converts feature class to a recordset.
        /// </summary>
        /// <param name="fc">The fc.</param>
        /// <returns></returns>
        public IRecordSet2 ConvertToRecordset(IFeatureClass fc)
        {
            IRecordSet recSet = new RecordSetClass();
            IRecordSetInit recSetInit = recSet as IRecordSetInit;
            recSetInit.SetSourceTable(fc as ITable, null);

            return (IRecordSet2)recSetInit;
        }

        #endregion

        #region Enums
        #endregion
    }
}
