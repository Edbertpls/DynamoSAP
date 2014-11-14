﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynamoSAP.Structure;
using DynamoSAP;

//DYNAMO
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

using SAPConnection;
using SAP2000v16;

namespace DynamoSAP.Assembly
{
    public class Read
    {
        //// DYNAMO NODES ////
        public static StructuralModel SAPModel(string FilePath, bool Run)
        {
            StructuralModel Model = new StructuralModel();
            Model.Frames = new List<Element>();
            cSapModel mySapModel = null;
            // Open & instantiate SAP file
            Initialize.OpenSAPModel(FilePath, ref mySapModel);

            // Populate the model's elemets

            //Get Frames          
            string[] FrmIds = null;
            StructureMapper.GetFrameIds(ref FrmIds, ref mySapModel);
            for (int i = 0; i < FrmIds.Length; i++)
            {
                Point s = null;
                Point e = null;
                string matProp = "Steel"; // default value
                string secProp = "W12X14"; // default value
                string Just = "MiddleCenter"; // default value
                double Rot = 0; // default value

                StructureMapper.GetFrm(ref mySapModel, FrmIds[i], ref s, ref e, ref matProp, ref secProp, ref Just, ref Rot);

                Frame d_frm = new Frame(s, e, matProp, secProp, Just, Rot);
                d_frm.Label = FrmIds[i];
                // get Guid
                string guid = string.Empty;
                StructureMapper.GetGUIDFrm(ref mySapModel, FrmIds[i], ref guid);
                d_frm.GUID = guid;

                Model.Frames.Add(d_frm);
            }


            return Model;
        }

        private Read() { }
    }
}
