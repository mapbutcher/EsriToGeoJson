using System;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using EsriToGeoJson;
using Xunit;

namespace EsriToGeoJsonTests
{
    public class EsriToGeoJsonTest
    {
        string _fgdb = @"C:\Data\IPhonePOCData.gdb";
        string _pointFeatureClassName = "Pubs";
        string _lineFeatureClassName = "RoadCentreLinesWGS84";
        string _polyFeatureClassName = "PropertyWGS84";
        IAoInitialize _license;
        IFeatureClass _fc;

        /// <summary>
        /// Create an instance of the test class.
        /// </summary>
        /// <remarks>Constructor contains necessary set up logic, including initialisation of esri license and opening test feature classes</remarks>
        public EsriToGeoJsonTest()
        {
            //initialise an esri license
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            if (!ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop))
            {
                throw new ApplicationException("Failed to initialise ArcGIS License");
            }

            _license = new AoInitialize();
            _license.Initialize(esriLicenseProductCode.esriLicenseProductCodeArcView);

        }


        [Fact]
        public void TestCreateExportPoint()
        {
            var export = new GeoJsonExport();

            //open point feature class and record set
            _fc = OpenFeatureClass(_fgdb, _pointFeatureClassName);
            IRecordSet2 recordSet = export.ConvertToRecordset(_fc);


            export.CreateExport(recordSet, "Shape");

            Console.WriteLine(export.GeoJson);

            WriteOutputToFile(export, @"C:\Git\EsriToGeoJson\LeafletTests\point.js");
        }



        [Fact]
        public void TestCreateExportLine()
        {
            var export = new GeoJsonExport();

            //open point feature class and record set
            _fc = OpenFeatureClass(_fgdb, _lineFeatureClassName);
            IRecordSet2 recordSet = export.ConvertToRecordset(_fc);


            export.CreateExport(recordSet, "Shape");

            //Console.WriteLine(export.GeoJson);

            WriteOutputToFile(export, @"C:\Git\EsriToGeoJson\LeafletTests\line.js");
        }

        [Fact]
        public void TestCreateExportPoly()
        {
            var export = new GeoJsonExport();

            //open point feature class and record set
            _fc = OpenFeatureClass(_fgdb, _polyFeatureClassName);
            IRecordSet2 recordSet = export.ConvertToRecordset(_fc);


            export.CreateExport(recordSet, "Shape");

            WriteOutputToFile(export, @"C:\Git\EsriToGeoJson\LeafletTests\poly.js");
        }

        private static void WriteOutputToFile(GeoJsonExport export, string outputFile)
        {
            // create a writer and open the file
            TextWriter tw = new StreamWriter(outputFile);

            // write a line of text to the file
            tw.WriteLine(export.GeoJson);

            // close the stream
            tw.Close();
        }

        private IFeatureClass OpenFeatureClass(string filePath, string fcName)
        {
            var gdbWorkspaceFactory = new FileGDBWorkspaceFactory();
            var featureWorkspace = (IFeatureWorkspace)gdbWorkspaceFactory.OpenFromFile(_fgdb, 0);
            return featureWorkspace.OpenFeatureClass(fcName);
        }


    }
}
