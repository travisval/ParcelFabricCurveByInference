using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParcelFabricCurveByInference;
using System.IO;

namespace ParcelFabricCurveByInferenceTest
{
    [TestClass]
    public class Sherwood
    {
        static string workspacePath;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            workspacePath = System.IO.Path.Combine(context.ResultsDirectory, "Working.gdb");
            DirectoryCopy(Framework.baseGeodatabasePath, workspacePath, false);
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (Directory.Exists(destDirName))
            {
                Directory.Delete(destDirName);
            }
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        #region Data

        static List<InferredCurve> data = new List<InferredCurve>() { 
             new InferredCurve(6, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5, 197.4707, 4, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(7, 197.4707, 4, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 197.4707, InferredCenterpointID = 4, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(8, "test", new List<RelatedCurve>() {
                  new RelatedCurve(7, 197.4707, 4, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(999, 89.9178996615324, 47.9211222398754, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(20, "test", new List<RelatedCurve>() {
                  new RelatedCurve(21, 1156.6589, 24, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(985, 1156.6589, 1320, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(4265, 1155.5022, 2062, CurveByInference.RelativeOrientation.To_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(19, 34.6525977196631, 89.8555792038292, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(987, 34.6525977196631, 89.8555792038292, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(4267, 34.6520633530284, 89.8561135704639, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(23, "test", new List<RelatedCurve>() {
                  new RelatedCurve(22, 1159.2021, 26, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(24, 1111.4165, 29, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(982, 1111.4165, 1318, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(984, 1159.2021, 1319, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(4262, 1110.305, 2060, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(4264, 1158.0429, 2061, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(235, "test", new List<RelatedCurve>() {
                  new RelatedCurve(234, 45.9862, 108, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(403, 45.9862, 108, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(236, 89.9110621583102, 0.0219340166019448, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(401, 89.9110621583102, 0.0219340166019448, CurveByInference.RelativeOrientation.To_To)
                  }},
             new InferredCurve(373, "test", new List<RelatedCurve>() {
                  new RelatedCurve(263, -192.1923, 143, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(374, -192.1923, 143, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -192.1923, InferredCenterpointID = 143, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(264, -179.244925544137, 90.3802308289896, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(372, 89.9984053378006, 0.376438289072462, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(402, "test", new List<RelatedCurve>() {
                  new RelatedCurve(234, -45.9862, 108, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(403, -45.9862, 108, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(236, -90.0889378416898, 0.0219340166019448, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(401, -90.0889378416898, 0.0219340166019448, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(594, "test", new List<RelatedCurve>() {
                  new RelatedCurve(595, -224.9381, 274, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(675, -224.9381, 274, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2502, -224.9381, 274, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2554, -224.9381, 274, CurveByInference.RelativeOrientation.To_To)     }){
                  InferredRadius = -224.9381, InferredCenterpointID = 274, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(593, 72.5590581489621, 0.00843994297339859, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(673, 72.5590581489621, 0.00843994297339859, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2504, 72.5590581489621, 0.00843994297339859, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2556, 72.5590581489621, 0.00843994297339859, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(674, "test", new List<RelatedCurve>() {
                  new RelatedCurve(595, -224.9381, 274, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(675, -224.9381, 274, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2502, -224.9381, 274, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2554, -224.9381, 274, CurveByInference.RelativeOrientation.To_To)     }){
                  InferredRadius = -224.9381, InferredCenterpointID = 274, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(593, 72.5590581489621, 0.00843994297339859, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(673, 72.5590581489621, 0.00843994297339859, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2504, 72.5590581489621, 0.00843994297339859, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2556, 72.5590581489621, 0.00843994297339859, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(891, "test", new List<RelatedCurve>() {
                  new RelatedCurve(890, -11.9964, 361, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2655, 11.997, 361, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2656, -11.997, 361, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(3138, 11.997, 361, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3139, -11.997, 361, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5525, 11.9964, 1282, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(892, -15.2312237993104, 0.334456360899505, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(934, -15.2312237993104, 0.334456360898416, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(5526, -105.202700687297, 90.3059332488855, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(935, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2655, 11.997, 361, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(3138, 11.997, 361, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5525, 11.9964, 1282, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 11.997, InferredCenterpointID = 361, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(891, -14.8967674624502, 4.15909827846918, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(892, 164.76877620069, 175.506445384671, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(934, 164.76877620069, 175.506445384671, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(936, -11.3408539746286, 0.603184790647571, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2654, -11.3408539746286, 0.603184790647571, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(3137, -11.3408539746286, 0.603184790647571, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(5524, -11.3408539746286, 0.603184790647571, CurveByInference.RelativeOrientation.To_To)
                  }},
             new InferredCurve(983, "test", new List<RelatedCurve>() {
                  new RelatedCurve(22, -1159.2021, 26, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(24, -1111.4165, 29, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(982, -1111.4165, 1318, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(984, -1159.2021, 1319, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(4262, -1110.305, 2060, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4264, -1158.0429, 2061, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(986, "test", new List<RelatedCurve>() {
                  new RelatedCurve(21, -1156.6589, 24, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(985, -1156.6589, 1320, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4265, -1155.5022, 2062, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(19, -145.347402280337, 89.8555792038292, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(987, -145.347402280337, 89.8555792038292, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(4267, -145.347936646972, 89.8561135704639, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(998, "test", new List<RelatedCurve>() {
                  new RelatedCurve(7, -197.4707, 4, CurveByInference.RelativeOrientation.To_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(999, -90.0821003384676, 47.9211222398753, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(1092, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1091, -287.379, 1340, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(1093, -284.249, 1344, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5316, -284.249, 1429, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5318, -287.379, 1203, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(8515, -284.249, 1429, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(8517, -287.379, 1203, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(8588, -284.249, 1429, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(8590, -287.379, 1203, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(1382, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1381, -342.2348, 548, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(1802, -342.2348, 548, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(6039, -342.2348, 548, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(6249, -342.2348, 548, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(1383, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1384, 174.9519, 667, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(1755, 174.9519, 667, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(6042, 174.9519, 667, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(6246, 174.9519, 667, CurveByInference.RelativeOrientation.To_To)     }){
                  InferredRadius = 174.9519, InferredCenterpointID = 667, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1382, 64.3123539022369, 1.32333364827652, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(1756, 152.870649045007, 89.8816287910464, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(1799, 152.870649045007, 89.8816287910464, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(1801, 64.3123539022369, 1.32333364827652, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(6040, 64.3123539022369, 1.32333364827652, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(6248, 64.3123539022369, 1.32333364827652, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(1397, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1398, -171.9527, 711, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(1591, -171.9527, 711, CurveByInference.RelativeOrientation.To_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(1592, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1398, 171.9527, 711, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(1591, 171.9527, 711, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(1611, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1612, 80.6632, 468, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2199, 80.6632, 468, CurveByInference.RelativeOrientation.To_To)     }){
                  InferredRadius = 80.6632, InferredCenterpointID = 468, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1610, -103.809421657036, 0.250736566134486, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2178, -19.5387918097314, 84.521366413439, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2180, -103.809421657036, 0.25073656613594, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2200, -19.5387918097314, 84.521366413439, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(1698, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1697, 588.0735, 689, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4982, 588.0735, 689, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(1797, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1796, -307.2378, 548, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4964, -307.2378, 548, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(1798, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1757, 209.937, 667, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(4967, 209.937, 667, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 209.937, InferredCenterpointID = 667, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1725, 156.345207616781, 93.3068059069486, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(1756, -27.1293509549933, 90.1677526648261, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(1763, 156.345207616781, 93.3068059069486, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(1797, 64.3128389409667, 1.27443723113422, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(1799, -27.1293509549933, 90.1677526648261, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(4965, 64.3128389409667, 1.27443723113422, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(1800, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1384, -174.9519, 667, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(1755, -174.9519, 667, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(6042, -174.9519, 667, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(6246, -174.9519, 667, CurveByInference.RelativeOrientation.From_To)     }){
                  InferredRadius = -174.9519, InferredCenterpointID = 667, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1382, -115.687646097763, 1.32333364827652, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(1756, -27.1293509549933, 89.8816287910464, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(1799, -27.1293509549933, 89.8816287910464, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(1801, -115.687646097763, 1.32333364827624, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(6040, -115.687646097763, 1.32333364827652, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(6248, -115.687646097763, 1.32333364827624, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(1801, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1381, 342.2348, 548, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(1802, 342.2348, 548, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(6039, 342.2348, 548, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(6249, 342.2348, 548, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(1934, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1938, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(1963, -64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2050, -64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2094, -64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5708, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5710, -64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5940, -64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5942, -64.9821, 498, CurveByInference.RelativeOrientation.From_From)     }){
                  InferredRadius = -64.9821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(2067, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2071, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2125, -64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2135, -64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2166, -64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5713, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5715, -64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5935, -64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5937, -64.9821, 498, CurveByInference.RelativeOrientation.From_From)     }){
                  InferredRadius = -64.9821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(2095, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1938, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(1963, 64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2050, 64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2094, 64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5708, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5710, 64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5940, 64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5942, 64.9821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 64.9821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(2164, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2156, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2165, 64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5716, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5934, 64.9821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2157, 171.847667380339, 89.7464771876308, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2163, 161.921424543465, 79.8202343507575, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2188, 171.847667380339, 89.7464771876308, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2213, 161.921424543465, 79.8202343507575, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(2167, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2071, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2125, 64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2135, 64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2166, 64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5713, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5715, 64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5935, 64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5937, 64.9821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 64.9821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(2179, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1612, -80.6632, 468, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2199, -80.6632, 468, CurveByInference.RelativeOrientation.From_To)     }){
                  InferredRadius = -80.6632, InferredCenterpointID = 468, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1610, 76.1905783429639, 0.25073656613594, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2178, 160.461208190269, 84.521366413439, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2180, 76.190578342964, 0.250736566134486, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2200, 160.461208190269, 84.521366413439, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(2189, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2156, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2165, -64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2191, 134.9629, 441, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2211, 134.9629, 441, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5716, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5719, 134.9629, 441, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5931, 134.9629, 441, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5934, -64.9821, 498, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2157, -8.1523326196614, 89.7464771876308, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2163, -18.0785754565347, 79.8202343507575, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2188, -8.1523326196614, 89.7464771876308, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2213, -18.0785754565347, 79.8202343507575, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(2409, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2427, -456.8629, 407, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(4397, -456.8629, 407, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -456.8629, InferredCenterpointID = 407, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2410, 127.431370210112, 89.9646740046207, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2413, -142.640958823552, 0.0370030382855798, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2431, 127.431370210112, 89.9646740046207, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(4395, -142.640958823552, 0.0370030382855798, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(2503, "test", new List<RelatedCurve>() {
                  new RelatedCurve(595, 224.9381, 274, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(675, 224.9381, 274, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2502, 224.9381, 274, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2554, 224.9381, 274, CurveByInference.RelativeOrientation.From_To)     }){
                  InferredRadius = 224.9381, InferredCenterpointID = 274, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(593, -107.440941851038, 0.0084399430165819, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(673, -107.440941851038, 0.0084399430165819, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2504, -107.440941851038, 0.0084399430165819, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2556, -107.440941851038, 0.0084399430165819, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(2555, "test", new List<RelatedCurve>() {
                  new RelatedCurve(595, 224.9381, 274, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(675, 224.9381, 274, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2502, 224.9381, 274, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2554, 224.9381, 274, CurveByInference.RelativeOrientation.From_To)     }){
                  InferredRadius = 224.9381, InferredCenterpointID = 274, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(593, -107.440941851038, 0.0084399430165819, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(673, -107.440941851038, 0.0084399430165819, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2504, -107.440941851038, 0.0084399430165819, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2556, -107.440941851038, 0.0084399430165819, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(2628, "test", new List<RelatedCurve>() {
                  new RelatedCurve(962, -311.3366, 1305, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -311.3366, InferredCenterpointID = 1305, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(2659, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2660, -783.98, 1205, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(3041, -782.533, 1013, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(7807, -783.7357, 1205, CurveByInference.RelativeOrientation.To_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2658, 90.0111489371508, 0.0806670430111064, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(3044, 15.6173632583453, 74.4744527218148, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(3091, 90.0111489371508, 0.0806670430111064, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(3093, 15.6173632583453, 74.4744527218148, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(7809, 90.0111489371508, 0.0806670430111064, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(2672, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2868, 414.564, 1065, CurveByInference.RelativeOrientation.Same)     }){
                  InferredRadius = 414.564, InferredCenterpointID = 1065, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(2680, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2679, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2681, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2916, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2926, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(3590, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3592, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(7786, 199.8837, 1037, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(7788, 401.4548, 1052, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2917, 80.2785104226404, 93.9770048985043, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2924, 80.2785104226404, 93.9770048985043, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(2685, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2684, 199.946, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2962, 199.946, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3595, 200.1459, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(7783, 199.8837, 1037, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2963, 78.3225860330751, 103.486047013716, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(3005, 78.3225860330751, 103.486047013716, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(2925, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2679, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2681, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2916, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2926, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(3590, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3592, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(7786, 199.8837, 1037, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(7788, 401.4548, 1052, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2917, 80.2785104226404, 93.9770048985043, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2924, 80.2785104226404, 93.9770048985043, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(3006, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2684, 199.946, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2962, 199.946, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3595, 200.1459, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(7783, 199.8837, 1037, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2963, 78.3225860330751, 103.486047013716, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(3005, 78.3225860330751, 103.486047013716, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(3092, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2660, -783.98, 1205, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(3041, -782.533, 1013, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(7807, -783.7357, 1205, CurveByInference.RelativeOrientation.To_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2658, 90.0111489371508, 0.0806670430111064, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(3044, 15.6173632583453, 74.4744527218148, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(3091, 90.0111489371508, 0.0806670430111064, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(3093, 15.6173632583453, 74.4744527218148, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(7809, 90.0111489371508, 0.0806670430111064, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(3591, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2679, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2681, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2916, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2926, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(3590, 401.58, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3592, 199.946, 1037, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(7786, 199.8837, 1037, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(7788, 401.4548, 1052, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2917, 80.2785104226404, 93.9770048985043, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2924, 80.2785104226404, 93.9770048985043, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(3596, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2684, 199.946, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2962, 199.946, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3595, 200.1459, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(7783, 199.8837, 1037, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2963, 78.3225860330751, 103.486047013716, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(3005, 78.3225860330751, 103.486047013716, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(3607, "test", new List<RelatedCurve>() {
                  new RelatedCurve(3608, -198.3459, 1037, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -198.3459, InferredCenterpointID = 1037, 
                  ParallelCurves = new List<RelatedCurve>() {
                        new RelatedCurve(2684, -198.131302430419, 1037, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(2962, -198.131302430419, 1037, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(3595, -198.331202430419, 1037, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(7783, -198.069002430419, 1037, CurveByInference.RelativeOrientation.Parallel)          },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(3616, "test", new List<RelatedCurve>() {
                  new RelatedCurve(3617, -399.78, 1052, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -399.78, InferredCenterpointID = 1052, 
                  ParallelCurves = new List<RelatedCurve>() {
                        new RelatedCurve(2679, -399.769340270349, 1052, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(2916, -399.769340270349, 1052, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(3590, -399.769340270349, 1052, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(7788, -399.644140270349, 1052, CurveByInference.RelativeOrientation.Parallel)          },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(3618, "test", new List<RelatedCurve>() {
                  new RelatedCurve(3617, -399.78, 1052, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3619, -399.78, 1052, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -399.78, InferredCenterpointID = 1052, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4000, "test", new List<RelatedCurve>() {
                  new RelatedCurve(978, -1087.9236, 1316, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -1087.9236, InferredCenterpointID = 1316, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4001, "test", new List<RelatedCurve>() {
                  new RelatedCurve(977, -1087.9235, 1315, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -1087.9235, InferredCenterpointID = 1315, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4002, "test", new List<RelatedCurve>() {
                  new RelatedCurve(976, -1087.9236, 1314, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -1087.9236, InferredCenterpointID = 1314, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4003, "test", new List<RelatedCurve>() {
                  new RelatedCurve(975, -1087.9235, 1313, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -1087.9235, InferredCenterpointID = 1313, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4004, "test", new List<RelatedCurve>() {
                  new RelatedCurve(974, -1087.9235, 1312, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -1087.9235, InferredCenterpointID = 1312, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4005, "test", new List<RelatedCurve>() {
                  new RelatedCurve(973, -1087.9236, 1311, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -1087.9236, InferredCenterpointID = 1311, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4006, "test", new List<RelatedCurve>() {
                  new RelatedCurve(972, -1087.9235, 1310, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -1087.9235, InferredCenterpointID = 1310, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4007, "test", new List<RelatedCurve>() {
                  new RelatedCurve(971, -1087.9235, 1309, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -1087.9235, InferredCenterpointID = 1309, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4008, "test", new List<RelatedCurve>() {
                  new RelatedCurve(970, 1087.9235, 1308, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(3960, 1087.9235, 1192, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2594, -154.335710055381, 88.787407243025, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(3495, -154.335710055381, 88.787407243025, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(3988, -154.335710055381, 88.787407243025, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(4009, 115.740006735019, 1.13687596657476, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(4256, "test", new List<RelatedCurve>() {
                  new RelatedCurve(4255, 441.8674, 407, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(4257, 37.3592761442178, 0.000532557694129756, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(4263, "test", new List<RelatedCurve>() {
                  new RelatedCurve(22, -1159.2021, 26, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(24, -1111.4165, 29, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(982, -1111.4165, 1318, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(984, -1159.2021, 1319, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(4262, -1110.305, 2060, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4264, -1158.0429, 2061, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4266, "test", new List<RelatedCurve>() {
                  new RelatedCurve(21, -1156.6589, 24, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(985, -1156.6589, 1320, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4265, -1155.5022, 2062, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(19, -145.347402280337, 89.8555792038292, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(987, -145.347402280337, 89.8555792038292, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(4267, -145.347936646972, 89.8561135704639, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(4268, "test", new List<RelatedCurve>() {
                  new RelatedCurve(4269, 1159.0022, 2062, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(19, 34.6525977196631, 89.8763771697621, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(987, 34.6525977196631, 89.8763771697621, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(4267, -145.347936646972, 90.1230884636032, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(4271, "test", new List<RelatedCurve>() {
                  new RelatedCurve(4270, 1161.5429, 2061, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4272, 1113.805, 2060, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4278, "test", new List<RelatedCurve>() {
                  new RelatedCurve(4279, -438.3674, 407, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(4277, -142.64315633661, 0.000164097871213501, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(4396, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2427, -456.8629, 407, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(4397, -456.8629, 407, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -456.8629, InferredCenterpointID = 407, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2410, 127.431370210112, 89.9646740046207, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2413, -142.640958823552, 0.0370030382855798, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2431, 127.431370210112, 89.9646740046207, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(4395, -142.640958823552, 0.0370030382855798, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(4446, "test", new List<RelatedCurve>() {
                  new RelatedCurve(4445, 460.3629, 407, CurveByInference.RelativeOrientation.From_To)     }){
                  InferredRadius = 460.3629, InferredCenterpointID = 407, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2410, -52.5686297898877, 90.4608894610623, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2431, -52.5686297898877, 90.4608894610623, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(4447, 37.3287314724488, 0.563528198725895, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(4491, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2345, -134.9629, 441, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -134.9629, InferredCenterpointID = 441, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4965, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1796, -307.2378, 548, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4964, -307.2378, 548, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4966, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1757, 209.937, 667, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(4967, 209.937, 667, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 209.937, InferredCenterpointID = 667, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1725, 156.345207616781, 93.3068059069486, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(1756, -27.1293509549933, 90.1677526648261, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(1763, 156.345207616781, 93.3068059069486, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(1797, 64.3128389409667, 1.27443723113422, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(1799, -27.1293509549933, 90.1677526648261, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(4965, 64.3128389409667, 1.27443723113422, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(4983, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1697, 588.0735, 689, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(4982, 588.0735, 689, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(4989, "test", new List<RelatedCurve>() {
                  new RelatedCurve(4990, -584.5735, 689, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(5007, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5008, 310.7378, 548, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(5297, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5298, -284.979, 1203, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -284.979, InferredCenterpointID = 1203, 
                  ParallelCurves = new List<RelatedCurve>() {
                        new RelatedCurve(960, -285.030371618053, 1304, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(2630, -285.03472037702, 1203, CurveByInference.RelativeOrientation.Parallel)          },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(5317, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1091, 287.379, 1340, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(1093, 284.249, 1344, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5316, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5318, 287.379, 1203, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(8515, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(8517, 287.379, 1203, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(8588, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(8590, 287.379, 1203, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(5563, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5564, 314.9798, 1203, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 314.9798, InferredCenterpointID = 1203, 
                  ParallelCurves = new List<RelatedCurve>() {
                        new RelatedCurve(1090, 314.922324080073, 1340, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(5298, 314.925377637624, 1203, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(5319, 314.92868841024, 1203, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(8518, 314.92868841024, 1203, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(8591, 314.92868841024, 1203, CurveByInference.RelativeOrientation.Parallel)          },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(5688, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5687, -133.1629, 441, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(5689, 82.1048526715586, 0.313650030468106, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(5689, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5690, 66.7821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 66.7821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2157, 171.847667380339, 89.7428147071045, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2188, 171.847667380339, 89.7428147071045, CurveByInference.RelativeOrientation.To_To)
                  }},
             new InferredCurve(5692, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5691, 66.7821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5693, 66.7821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 66.7821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2068, -24.0482928907282, 89.6863986917886, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2128, -24.0482928907282, 89.6863986917886, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(5697, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5696, 66.7821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5698, 66.7821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 66.7821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1935, -49.2011219287399, 89.7576773283378, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(1966, -49.2011219287399, 89.7576773283378, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(5709, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1938, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(1963, -64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2050, -64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2094, -64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5708, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5710, -64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5940, -64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5942, -64.9821, 498, CurveByInference.RelativeOrientation.From_From)     }){
                  InferredRadius = -64.9821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(5714, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2071, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2125, -64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2135, -64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2166, -64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5713, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5715, -64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5935, -64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5937, -64.9821, 498, CurveByInference.RelativeOrientation.From_From)     }){
                  InferredRadius = -64.9821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(5717, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2156, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(2165, -64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2191, 134.9629, 441, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2211, 134.9629, 441, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5716, -64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5719, 134.9629, 441, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5931, 134.9629, 441, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5934, -64.9821, 498, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2157, -8.1523326196614, 89.7464771876308, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(2163, -18.0785754565347, 79.8202343507575, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2188, -8.1523326196614, 89.7464771876308, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2213, -18.0785754565347, 79.8202343507575, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(5873, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5874, -64.3821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(5872, -154.302025714816, 0.00112517649462697, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(5877, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5876, -64.3821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5878, -64.3821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -64.3821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2049, 132.947824771665, 87.695333157974, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2096, 132.947824771665, 87.695333157974, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(5885, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5884, -64.3821, 498, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2163, -18.0785754565347, 81.3002475675749, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2213, -18.0785754565347, 81.3002475675749, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(5933, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2156, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2165, 64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(5716, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5934, 64.9821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2157, 171.847667380339, 89.7464771876308, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2163, 161.921424543465, 79.8202343507575, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(2188, 171.847667380339, 89.7464771876308, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(2213, 161.921424543465, 79.8202343507575, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(5936, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2071, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2125, 64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2135, 64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2166, 64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5713, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5715, 64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5935, 64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5937, 64.9821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 64.9821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(5941, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1938, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(1963, 64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2050, 64.9821, 498, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(2094, 64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5708, 64.9821, 498, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(5710, 64.9821, 498, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5940, 64.9821, 498, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5942, 64.9821, 498, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 64.9821, InferredCenterpointID = 498, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(6040, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1381, -342.2348, 548, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(1802, -342.2348, 548, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(6039, -342.2348, 548, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(6249, -342.2348, 548, CurveByInference.RelativeOrientation.From_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(6041, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1384, 174.9519, 667, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(1755, 174.9519, 667, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(6042, 174.9519, 667, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(6246, 174.9519, 667, CurveByInference.RelativeOrientation.To_To)     }){
                  InferredRadius = 174.9519, InferredCenterpointID = 667, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1382, 64.3123539022369, 1.32333364827652, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(1756, 152.870649045007, 89.8816287910464, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(1799, 152.870649045007, 89.8816287910464, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(1801, 64.3123539022369, 1.32333364827652, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(6040, 64.3123539022369, 1.32333364827652, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(6248, 64.3123539022369, 1.32333364827652, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(6054, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6055, -174.3519, 667, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(6053, -143.836266725132, 0.000176994916597562, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(6064, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6065, 342.8348, 548, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(6191, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1468, -114.9684, 662, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(1567, 114.9684, 662, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(4933, 115.0833, 1562, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5245, -114.9684, 662, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(6416, -114.9684, 662, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1469, -16.4975382602642, 88.9911856673374, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(1566, -16.4975382602642, 88.9911856673373, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(5041, -16.4975382602642, 88.9911856673373, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(5246, -16.4975382602642, 88.9911856673374, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(6190, -16.4975382602642, 88.9911856673373, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(6192, 163.571514430594, 90.9397616418048, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(6417, -16.4975382602642, 88.9911856673374, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(6224, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6223, -340.4348, 548, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(6225, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6226, 176.7519, 667, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(6224, 64.2961080981024, 1.70239949561726, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(6247, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1384, -174.9519, 667, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(1755, -174.9519, 667, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(6042, -174.9519, 667, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(6246, -174.9519, 667, CurveByInference.RelativeOrientation.From_To)     }){
                  InferredRadius = -174.9519, InferredCenterpointID = 667, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(1382, -115.687646097763, 1.32333364827652, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(1756, -27.1293509549933, 89.8816287910464, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(1799, -27.1293509549933, 89.8816287910464, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(1801, -115.687646097763, 1.32333364827624, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(6040, -115.687646097763, 1.32333364827652, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(6248, -115.687646097763, 1.32333364827624, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(6248, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1381, 342.2348, 548, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(1802, 342.2348, 548, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(6039, 342.2348, 548, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(6249, 342.2348, 548, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(6642, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6641, 245.6824, 132, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(6643, 17.995, 1447, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(6682, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6683, -242.1824, 132, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -242.1824, InferredCenterpointID = 132, 
                  ParallelCurves = new List<RelatedCurve>() {
                        new RelatedCurve(436, -242.309343606529, 149, CurveByInference.RelativeOrientation.Parallel)          },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(6946, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6947, 147.961, 268, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(7265, 147.961, 268, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 147.961, InferredCenterpointID = 268, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(6945, 90.8402328988281, 0.0123043016632288, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(7264, -87.9233163789873, 178.751244976135, CurveByInference.RelativeOrientation.To_To)
                  }},
             new InferredCurve(6951, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6952, -218.6881, 274, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(6950, 72.5597119843968, 0.12482687924633, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(7270, 72.5598480380733, 0.124690825573825, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(7271, -111.967906792024, 175.347554344331, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(6954, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6953, -218.6881, 274, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(7380, 216.75, 2417, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(6956, 90.8718686665767, 3.92864193776223, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(7305, 90.8718686665767, 3.92864193776223, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(7379, 90.7859575303824, 3.84273080156796, CurveByInference.RelativeOrientation.To_To)
                  }},
             new InferredCurve(6964, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6963, 231.9381, 274, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(7104, 231.9381, 2211, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(6965, -107.440941851038, 0.000272111470572607, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(7106, -107.441126658317, 0.000456917289631504, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(7105, "test", new List<RelatedCurve>() {
                  new RelatedCurve(6963, 231.9381, 274, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(7104, 231.9381, 2211, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(6964, 72.5593302663424, 179.99852407747, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(6965, -107.440941851038, 0.00120380509779011, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(7106, -107.441126658317, 0.00101899771407189, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(7782, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2684, -199.946, 1037, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2962, -199.946, 1037, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(3595, -200.1459, 1037, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(7783, -199.8837, 1037, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2963, -101.677413966925, 103.486047013716, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(3005, -101.677413966925, 103.486047013716, CurveByInference.RelativeOrientation.To_To)
                  }},
             new InferredCurve(7787, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2679, -401.58, 1052, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2681, -199.946, 1037, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(2916, -401.58, 1052, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(2926, -199.946, 1037, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(3590, -401.58, 1052, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(3592, -199.946, 1037, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(7786, -199.8837, 1037, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(7788, -401.4548, 1052, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2917, -99.7214895773596, 93.9770048985043, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(2924, -99.7214895773596, 93.9770048985043, CurveByInference.RelativeOrientation.To_To)
                  }},
             new InferredCurve(7795, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2868, -414.564, 1065, CurveByInference.RelativeOrientation.Reverse)     }){
                  InferredRadius = -414.564, InferredCenterpointID = 1065, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(7808, "test", new List<RelatedCurve>() {
                  new RelatedCurve(2660, 783.98, 1205, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(3041, 782.533, 1013, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(7807, 783.7357, 1205, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(2658, -89.9888510628492, 0.0806670430111064, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(3044, -164.382636741655, 74.4744527218148, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(3091, -89.9888510628492, 0.0806670430111064, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(3093, -164.382636741655, 74.4744527218148, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(7809, -89.9888510628492, 0.0806670430111064, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(8063, "test", new List<RelatedCurve>() {
                  new RelatedCurve(8064, 214.749, 2791, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(8364, 214.749, 2791, CurveByInference.RelativeOrientation.To_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(8285, "test", new List<RelatedCurve>() {
                  new RelatedCurve(8064, -214.749, 2791, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(8364, -214.749, 2791, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(8320, "test", new List<RelatedCurve>() {
                  new RelatedCurve(8319, -466.038, 2724, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(8321, -23.6223347988668, 0.000676816908312212, CurveByInference.RelativeOrientation.To_From)
                  }},
             new InferredCurve(8327, "test", new List<RelatedCurve>() {
                  new RelatedCurve(8328, 639.592, 2755, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 639.592, InferredCenterpointID = 2755, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(8326, -23.6222717252827, 0.00113615300436192, CurveByInference.RelativeOrientation.From_To)
                  }},
             new InferredCurve(8516, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1091, 287.379, 1340, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(1093, 284.249, 1344, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5316, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5318, 287.379, 1203, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(8515, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(8517, 287.379, 1203, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(8588, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(8590, 287.379, 1203, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(8589, "test", new List<RelatedCurve>() {
                  new RelatedCurve(1091, 287.379, 1340, CurveByInference.RelativeOrientation.To_To),
                  new RelatedCurve(1093, 284.249, 1344, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(5316, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(5318, 287.379, 1203, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(8515, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(8517, 287.379, 1203, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(8588, 284.249, 1429, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(8590, 287.379, 1203, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(8668, "test", new List<RelatedCurve>() {
                  new RelatedCurve(8667, -7.7979, 2836, CurveByInference.RelativeOrientation.From_To),
                  new RelatedCurve(8669, -7.7979, 2836, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -7.7979, InferredCenterpointID = 2836, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(8896, "test", new List<RelatedCurve>() {
                  new RelatedCurve(8897, -633.802, 2712, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(9047, "test", new List<RelatedCurve>() {
                  new RelatedCurve(9048, 410, 3086, CurveByInference.RelativeOrientation.To_From)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(9116, "test", new List<RelatedCurve>() {
                  new RelatedCurve(9117, 17.075, 3250, CurveByInference.RelativeOrientation.To_From),
                  new RelatedCurve(9280, 17.075, 3242, CurveByInference.RelativeOrientation.To_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(9115, 89.9181285968029, 0.183630817385795, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(9277, 179.918341539012, 90.1838437595947, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(9291, 179.918341539012, 90.1838437595947, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(9293, 89.9181285968029, 0.183630817385795, CurveByInference.RelativeOrientation.From_From)
                  }},
             new InferredCurve(9120, "test", new List<RelatedCurve>() {
                  new RelatedCurve(8900, -632.047, 2712, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = -632.047, InferredCenterpointID = 2712, 
                  ParallelCurves = new List<RelatedCurve>() {
                        new RelatedCurve(8290, -631.772354593233, 2712, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(8891, -631.772354593233, 2712, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(8897, -631.773786369349, 2712, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(9124, -631.772354593233, 2712, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(9462, -631.770354593233, 2712, CurveByInference.RelativeOrientation.Parallel)          },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(9228, "test", new List<RelatedCurve>() {
                  new RelatedCurve(9227, 17.075, 3310, CurveByInference.RelativeOrientation.From_To)     }){
                  InferredRadius = 17.075, InferredCenterpointID = 3310, 
                  ParallelCurves = new List<RelatedCurve>() {
                        new RelatedCurve(8948, 17.0735896545651, 3004, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(8958, 17.0701487272071, 3004, CurveByInference.RelativeOrientation.Parallel)          },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(9266, "test", new List<RelatedCurve>() {
                  new RelatedCurve(8900, 632.047, 2712, CurveByInference.RelativeOrientation.From_From)     }){
                  InferredRadius = 632.047, InferredCenterpointID = 2712, 
                  ParallelCurves = new List<RelatedCurve>() {
                        new RelatedCurve(8290, 631.772354593233, 2712, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(8891, 631.772354593233, 2712, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(8897, 631.773786369349, 2712, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(9124, 631.772354593233, 2712, CurveByInference.RelativeOrientation.Parallel),
                        new RelatedCurve(9462, 631.770354593233, 2712, CurveByInference.RelativeOrientation.Parallel)          },
                  TangentLines = new List<RelatedLine>() {

                  }},
             new InferredCurve(9292, "test", new List<RelatedCurve>() {
                  new RelatedCurve(9117, -17.075, 3250, CurveByInference.RelativeOrientation.From_From),
                  new RelatedCurve(9280, -17.075, 3242, CurveByInference.RelativeOrientation.From_To)     }){
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(9115, -90.0818714031971, 0.183630817385795, CurveByInference.RelativeOrientation.To_To),
                       new RelatedLine(9277, -0.0816584609882331, 90.1838437595947, CurveByInference.RelativeOrientation.From_From),
                       new RelatedLine(9291, -0.0816584609882422, 90.1838437595947, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(9293, -90.0818714031971, 0.183630817385795, CurveByInference.RelativeOrientation.To_From)
                  }}};

        #endregion

        void RunTest(params int[] ObjectIDs)
        {
            CurveByInference result = Framework.RunTest("AddinSherwood", "ParcelFabric", String.Format("Objectid in ({0})", string.Join(",", ObjectIDs)), null, workspacePath, true);

            List<InferredCurve> expectedResults = (from InferredCurve curve in data where ObjectIDs.Contains(curve.ObjectID) select curve).ToList();

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        //[TestMethod]
        //[TestCategory("Long")]
        //public void TestMethod_All()
        //{
        //    CurveByInference result = Framework.RunTest("AddinSherwood", "ParcelFabric");
            
        //    Framework.AssertInferredCurvesAreEqual(data, result.Curves);
        //}

        [TestMethod]
        public void SherwoodTest_6()
        {
            RunTest(6);
        }
        [TestMethod]
        public void SherwoodTest_8()
        {
            RunTest(8);
        }
        [TestMethod]
        public void SherwoodTest_20()
        {
            RunTest(20);
        }
        [TestMethod]
        public void SherwoodTest_23()
        {
            RunTest(23);
        }
        [TestMethod]
        public void SherwoodTest_235()
        {
            RunTest(235);
        }
        [TestMethod]
        public void SherwoodTest_373()
        {
            RunTest(373);
        }
        [TestMethod]
        public void SherwoodTest_402()
        {
            RunTest(402);
        }
        [TestMethod]
        public void SherwoodTest_594()
        {
            RunTest(594);
        }
        [TestMethod]
        public void SherwoodTest_674()
        {
            RunTest(674);
        }
        [TestMethod]
        public void SherwoodTest_891()
        {
            RunTest(891);
        }
        [TestMethod]
        public void SherwoodTest_935()
        {
            RunTest(935);
        }
        [TestMethod]
        public void SherwoodTest_983()
        {
            RunTest(983);
        }
        [TestMethod]
        public void SherwoodTest_986()
        {
            RunTest(986);
        }
        [TestMethod]
        public void SherwoodTest_998()
        {
            RunTest(998);
        }
        [TestMethod]
        public void SherwoodTest_1092()
        {
            RunTest(1092);
        }
        [TestMethod]
        public void SherwoodTest_1382()
        {
            RunTest(1382);
        }
        [TestMethod]
        public void SherwoodTest_1383()
        {
            RunTest(1383);
        }
        [TestMethod]
        public void SherwoodTest_1397()
        {
            RunTest(1397);
        }
        [TestMethod]
        public void SherwoodTest_1592()
        {
            RunTest(1592);
        }
        [TestMethod]
        public void SherwoodTest_1611()
        {
            RunTest(1611);
        }
        [TestMethod]
        public void SherwoodTest_1698()
        {
            RunTest(1698);
        }
        [TestMethod]
        public void SherwoodTest_1797()
        {
            RunTest(1797);
        }
        [TestMethod]
        public void SherwoodTest_1798()
        {
            RunTest(1798);
        }
        [TestMethod]
        public void SherwoodTest_1800()
        {
            RunTest(1800);
        }
        [TestMethod]
        public void SherwoodTest_1801()
        {
            RunTest(1801);
        }
        [TestMethod]
        public void SherwoodTest_1934()
        {
            RunTest(1934);
        }
        [TestMethod]
        public void SherwoodTest_2067()
        {
            RunTest(2067);
        }
        [TestMethod]
        public void SherwoodTest_2095()
        {
            RunTest(2095);
        }
        [TestMethod]
        public void SherwoodTest_2164()
        {
            RunTest(2164);
        }
        [TestMethod]
        public void SherwoodTest_2167()
        {
            RunTest(2167);
        }
        [TestMethod]
        public void SherwoodTest_2179()
        {
            RunTest(2179);
        }
        [TestMethod]
        public void SherwoodTest_2189()
        {
            RunTest(2189);
        }
        [TestMethod]
        public void SherwoodTest_2409()
        {
            RunTest(2409);
        }
        [TestMethod]
        public void SherwoodTest_2503()
        {
            RunTest(2503);
        }
        [TestMethod]
        public void SherwoodTest_2555()
        {
            RunTest(2555);
        }
        [TestMethod]
        public void SherwoodTest_2659()
        {
            RunTest(2659);
        }
        [TestMethod]
        public void SherwoodTest_2672()
        {
            RunTest(2672);
        }
        [TestMethod]
        public void SherwoodTest_2680()
        {
            RunTest(2680);
        }
        [TestMethod]
        public void SherwoodTest_2685()
        {
            RunTest(2685);
        }
        [TestMethod]
        public void SherwoodTest_2925()
        {
            RunTest(2925);
        }
        [TestMethod]
        public void SherwoodTest_3006()
        {
            RunTest(3006);
        }
        [TestMethod]
        public void SherwoodTest_3092()
        {
            RunTest(3092);
        }
        [TestMethod]
        public void SherwoodTest_3458()
        {
            RunTest(3458);
        }
        [TestMethod]
        public void SherwoodTest_3591()
        {
            RunTest(3591);
        }
        [TestMethod]
        public void SherwoodTest_3596()
        {
            RunTest(3596);
        }
        [TestMethod]
        public void SherwoodTest_3607()
        {
            RunTest(3607);
        }
        [TestMethod]
        public void SherwoodTest_3616()
        {
            RunTest(3616);
        }
        [TestMethod]
        public void SherwoodTest_3618()
        {
            RunTest(3618);
        }
        [TestMethod]
        public void SherwoodTest_3626()
        {
            RunTest(3626);
        }
        [TestMethod]
        public void SherwoodTest_4000()
        {
            RunTest(4000);
        }
        [TestMethod]
        public void SherwoodTest_4001()
        {
            RunTest(4001);
        }
        [TestMethod]
        public void SherwoodTest_4002()
        {
            RunTest(4002);
        }
        [TestMethod]
        public void SherwoodTest_4003()
        {
            RunTest(4003);
        }
        [TestMethod]
        public void SherwoodTest_4004()
        {
            RunTest(4004);
        }
        [TestMethod]
        public void SherwoodTest_4005()
        {
            RunTest(4005);
        }
        [TestMethod]
        public void SherwoodTest_4006()
        {
            RunTest(4006);
        }
        [TestMethod]
        public void SherwoodTest_4007()
        {
            RunTest(4007);
        }
        [TestMethod]
        public void SherwoodTest_4008()
        {
            RunTest(4008);
        }
        [TestMethod]
        public void SherwoodTest_4256()
        {
            RunTest(4256);
        }
        [TestMethod]
        public void SherwoodTest_4263()
        {
            RunTest(4263);
        }
        [TestMethod]
        public void SherwoodTest_4266()
        {
            RunTest(4266);
        }
        [TestMethod]
        public void SherwoodTest_4268()
        {
            RunTest(4268);
        }
        [TestMethod]
        public void SherwoodTest_4271()
        {
            RunTest(4271);
        }
        [TestMethod]
        public void SherwoodTest_4278()
        {
            RunTest(4278);
        }
        [TestMethod]
        public void SherwoodTest_4396()
        {
            RunTest(4396);
        }
        [TestMethod]
        public void SherwoodTest_4446()
        {
            RunTest(4446);
        }
        [TestMethod]
        public void SherwoodTest_4474()
        {
            RunTest(4474);
        }
        [TestMethod]
        public void SherwoodTest_4491()
        {
            RunTest(4491);
        }
        [TestMethod]
        public void SherwoodTest_4717()
        {
            RunTest(4717);
        }
        [TestMethod]
        public void SherwoodTest_4965()
        {
            RunTest(4965);
        }
        [TestMethod]
        public void SherwoodTest_4966()
        {
            RunTest(4966);
        }
        [TestMethod]
        public void SherwoodTest_4983()
        {
            RunTest(4983);
        }
        [TestMethod]
        public void SherwoodTest_4989()
        {
            RunTest(4989);
        }
        [TestMethod]
        public void SherwoodTest_5007()
        {
            RunTest(5007);
        }
        [TestMethod]
        public void SherwoodTest_5274()
        {
            RunTest(5274);
        }
        [TestMethod]
        public void SherwoodTest_5297()
        {
            RunTest(5297);
        }
        [TestMethod]
        public void SherwoodTest_5317()
        {
            RunTest(5317);
        }
        [TestMethod]
        public void SherwoodTest_5563()
        {
            RunTest(5563);
        }
        [TestMethod]
        public void SherwoodTest_5688()
        {
            RunTest(5688);
        }
        [TestMethod]
        public void SherwoodTest_5689()
        {
            RunTest(5689);
        }
        [TestMethod]
        public void SherwoodTest_5692()
        {
            RunTest(5692);
        }
        [TestMethod]
        public void SherwoodTest_5697()
        {
            RunTest(5697);
        }
        [TestMethod]
        public void SherwoodTest_5701()
        {
            RunTest(5701);
        }
        [TestMethod]
        public void SherwoodTest_5709()
        {
            RunTest(5709);
        }
        [TestMethod]
        public void SherwoodTest_5714()
        {
            RunTest(5714);
        }
        [TestMethod]
        public void SherwoodTest_5717()
        {
            RunTest(5717);
        }
        [TestMethod]
        public void SherwoodTest_5873()
        {
            RunTest(5873);
        }
        [TestMethod]
        public void SherwoodTest_5877()
        {
            RunTest(5877);
        }
        [TestMethod]
        public void SherwoodTest_5885()
        {
            RunTest(5885);
        }
        [TestMethod]
        public void SherwoodTest_5933()
        {
            RunTest(5933);
        }
        [TestMethod]
        public void SherwoodTest_5936()
        {
            RunTest(5936);
        }
        [TestMethod]
        public void SherwoodTest_5941()
        {
            RunTest(5941);
        }
        [TestMethod]
        public void SherwoodTest_6040()
        {
            RunTest(6040);
        }
        [TestMethod]
        public void SherwoodTest_6041()
        {
            RunTest(6041);
        }
        [TestMethod]
        public void SherwoodTest_6054()
        {
            RunTest(6054);
        }
        [TestMethod]
        public void SherwoodTest_6064()
        {
            RunTest(6064);
        }
        [TestMethod]
        public void SherwoodTest_6191()
        {
            RunTest(6191);
        }
        [TestMethod]
        public void SherwoodTest_6224()
        {
            RunTest(6224);
        }
        [TestMethod]
        public void SherwoodTest_6225()
        {
            RunTest(6225);
        }
        [TestMethod]
        public void SherwoodTest_6234()
        {
            RunTest(6234);
        }
        [TestMethod]
        public void SherwoodTest_6247()
        {
            RunTest(6247);
        }
        [TestMethod]
        public void SherwoodTest_6248()
        {
            RunTest(6248);
        }
        [TestMethod]
        public void SherwoodTest_6395()
        {
            RunTest(6395);
        }
        [TestMethod]
        public void SherwoodTest_6543()
        {
            RunTest(6543);
        }
        [TestMethod]
        public void SherwoodTest_6642()
        {
            RunTest(6642);
        }
        [TestMethod]
        public void SherwoodTest_6682()
        {
            RunTest(6682);
        }
        [TestMethod]
        public void SherwoodTest_6946()
        {
            RunTest(6946);
        }
        [TestMethod]
        public void SherwoodTest_6951()
        {
            RunTest(6951);
        }
        [TestMethod]
        public void SherwoodTest_6954()
        {
            RunTest(6954);
        }
        [TestMethod]
        public void SherwoodTest_6955()
        {
            RunTest(6955);
        }
        [TestMethod]
        public void SherwoodTest_6964()
        {
            RunTest(6964);
        }
        [TestMethod]
        public void SherwoodTest_7105()
        {
            RunTest(7105);
        }
        [TestMethod]
        public void SherwoodTest_7782()
        {
            RunTest(7782);
        }
        [TestMethod]
        public void SherwoodTest_7787()
        {
            RunTest(7787);
        }
        [TestMethod]
        public void SherwoodTest_7795()
        {
            RunTest(7795);
        }
        [TestMethod]
        public void SherwoodTest_7808()
        {
            RunTest(7808);
        }
        [TestMethod]
        public void SherwoodTest_8063()
        {
            RunTest(8063);
        }
        [TestMethod]
        public void SherwoodTest_8285()
        {
            RunTest(8285);
        }
        [TestMethod]
        public void SherwoodTest_8320()
        {
            RunTest(8320);
        }
        [TestMethod]
        public void SherwoodTest_8327()
        {
            RunTest(8327);
        }
        [TestMethod]
        public void SherwoodTest_8516()
        {
            RunTest(8516);
        }
        [TestMethod]
        public void SherwoodTest_8589()
        {
            RunTest(8589);
        }
        [TestMethod]
        public void SherwoodTest_8668()
        {
            RunTest(8668);
        }
        [TestMethod]
        public void SherwoodTest_8896()
        {
            RunTest(8896);
        }
        [TestMethod]
        public void SherwoodTest_9047()
        {
            RunTest(9047);
        }
        [TestMethod]
        public void SherwoodTest_9116()
        {
            RunTest(9116);
        }
        [TestMethod]
        public void SherwoodTest_9120()
        {
            RunTest(9120);
        }
        [TestMethod]
        public void SherwoodTest_9228()
        {
            RunTest(9228);
        }
        [TestMethod]
        public void SherwoodTest_9266()
        {
            RunTest(9266);
        }
        [TestMethod]
        public void SherwoodTest_9292()
        {
            RunTest(9292);
        }
        [TestMethod]
        public void SherwoodTest_9470()
        {
            RunTest(9470);
        }
        [TestMethod]
        public void SherwoodTest_9475()
        {
            RunTest(9475);
        }


    }
}
