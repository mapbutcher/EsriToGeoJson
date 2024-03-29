﻿#region File Information
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
using ESRI.ArcGIS.Geodatabase;
namespace EsriToGeoJson
{
    /// <summary>
    /// 
    /// </summary>
    class GeoJsonAttributeFactory
    {
        #region Member Variables
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Public Methods

        public Dictionary<string, object> GetAttributes(IRow row)
        {
            var atts = new Dictionary<string, object>();
            for (int i = 0; i < row.Fields.FieldCount; i++)
            {
                esriFieldType esriType = row.Fields.get_Field(i).Type;
                switch (esriType)
                {
                    case esriFieldType.esriFieldTypeBlob:
                        break;
                    case esriFieldType.esriFieldTypeDate:
                        DateTime date = (DateTime)row.get_Value(i);
                        atts.Add(row.Fields.get_Field(i).AliasName, date.Date.ToShortDateString());
                        break;
                    case esriFieldType.esriFieldTypeDouble:
                        atts.Add(row.Fields.get_Field(i).AliasName, row.get_Value(i));
                        break;
                    case esriFieldType.esriFieldTypeGUID:
                        break;
                    case esriFieldType.esriFieldTypeGeometry:
                        break;
                    case esriFieldType.esriFieldTypeGlobalID:
                        break;
                    case esriFieldType.esriFieldTypeInteger:
                        atts.Add(row.Fields.get_Field(i).AliasName, row.get_Value(i));
                        break;
                    case esriFieldType.esriFieldTypeOID:
                        break;
                    case esriFieldType.esriFieldTypeRaster:
                        break;
                    case esriFieldType.esriFieldTypeSingle:
                        atts.Add(row.Fields.get_Field(i).AliasName, row.get_Value(i));
                        break;
                    case esriFieldType.esriFieldTypeSmallInteger:
                        atts.Add(row.Fields.get_Field(i).AliasName, row.get_Value(i));
                        break;
                    case esriFieldType.esriFieldTypeString:
                        atts.Add(row.Fields.get_Field(i).AliasName, row.get_Value(i));
                        break;
                    case esriFieldType.esriFieldTypeXML:
                        break;
                    default:
                        break;
                }



            }

            return atts;
        }

        #endregion

        #region Private Methods
        #endregion

        #region Enums
        #endregion
    }
}
