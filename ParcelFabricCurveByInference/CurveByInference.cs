using ESRI.ArcGIS.CadastralUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeoDatabaseExtensions;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ParcelFabricCurveByInference
{
    public class CurveByInference : INotifyPropertyChanged
    {
        IEditor m_pEd = null;

        public ObservableCollection<InferredCurve> Curves { get; private set; }

        public MyMessageBox messageBox = new FormsMessageBox();

        public InferredCurve _SelectedItem;
        public InferredCurve SelectedItem { 
            get { return _SelectedItem; } 
            set { _SelectedItem = value; RaisePropertyChanged("SelectedItem"); } }

        bool _Finished;
        public bool Finished { get { return _Finished; } set { _Finished = value; RaisePropertyChanged("Finished", "IsEditing"); } }
        
        int _Total = 0;
        public int Total { get { return _Total; } private set { _Total = value; RaisePropertyChanged("Total"); } }
        int _Inferred = 0;
        public int Inferred { get { return _Inferred; } private set { _Inferred = value; RaisePropertyChanged("Inferred"); } }

        bool _IsEditing = false;
        public bool IsEditing { get { return Finished && _IsEditing; } private set { _IsEditing = value; RaisePropertyChanged("IsEditing"); } }
        void editEvent_OnStopEditing(bool save) { IsEditing = false; } //this event fires before IEditor actually closes the edit session
        void editEvent_OnStartEditing() { IsEditing = true; }

        public CurveByInference()
        {
            Finished = true;
            Curves = new ObservableCollection<InferredCurve>();

            m_pEd = (IEditor)ArcMap.Application.FindExtensionByName("esri object editor");
            _IsEditing = m_pEd.EditState == esriEditState.esriStateEditing;
            IEditEvents_Event editEvent = (IEditEvents_Event)m_pEd;

            editEvent.OnStartEditing += new IEditEvents_OnStartEditingEventHandler(editEvent_OnStartEditing);
            editEvent.OnStopEditing += new IEditEvents_OnStopEditingEventHandler(editEvent_OnStopEditing);
        }

        public void FindCurves(string whereClause = null)
        {
            IMap pMap = ArcMap.Document.ActiveView.FocusMap;

            if (hasOpenJob())
                return;

            IRelationshipClass relationshipClass = null;
            ISelectionSet relatedSelectionSet = null;

            myAOProgressor progressor = new myAOProgressor();
            try
            {
                Finished = false;

                progressor.setStepProgressorProperties(1, "Initializing");
                ICadastralFabricLayer CFLayer = GetFabricLayer(pMap);

                IFeatureLayer CFLineLayer = CFLayer.get_CadastralSubLayer(esriCadastralFabricRenderer.esriCFRLines);
                IFeatureLayer CFParcelLayer = CFLayer.get_CadastralSubLayer(esriCadastralFabricRenderer.esriCFRParcels);

                //Verify that the needed fields are present                                
                int idxParcelIDFld = CFLineLayer.FeatureClass.Fields.FindField("ParcelID");
                int idxCENTERPTID = CFLineLayer.FeatureClass.Fields.FindField("CenterPointID");
                int idxRADIUS = CFLineLayer.FeatureClass.Fields.FindField("Radius");
                if (idxParcelIDFld == -1 || idxCENTERPTID == -1 || idxRADIUS == -1)
                {
                    messageBox.Show("One or more of the following fields are missing (ParcelID, CenterPointID, Radius).");
                    return;
                }
                    
                //for each CF line feature class 
                IFeatureSelection CFLineFeatureSelection = (IFeatureSelection)CFLineLayer;
                IFeatureSelection CFParcelFeatureSelection = (IFeatureSelection)CFParcelLayer;

                ISelectionSet selectionSet = null;
                if (CFParcelFeatureSelection.SelectionSet.Count > 0)
                {
                    //parcels selected
                        
                    //Create memory relationship class to help with the mapping
                    IMemoryRelationshipClassFactory MemoryRCF = new MemoryRelationshipClassFactoryClass();
                    relationshipClass = MemoryRCF.Open("Parcel_Layer_Rel", CFParcelLayer.FeatureClass, "ObjectID", CFLineLayer.FeatureClass, "ParcelID", "forward", "backward", esriRelCardinality.esriRelCardinalityOneToMany);

                    //convert selection set to ISet of rows
                    ISet polySet = new Set();
                    ICursor cursor;
                    CFParcelFeatureSelection.SelectionSet.Search(null, false, out cursor);
                    IRow row = null;
                    while((row = cursor.NextRow()) != null)
                        polySet.Add(row);
                    polySet.Reset();

                    //use the relationship class to find related rows
                    ISet lineSet = relationshipClass.GetObjectsRelatedToObjectSet(polySet);

                    //convert set back to selection set
                    lineSet.Reset();
                        
                    //create an empty selection set (there should be a better way to do this....
                        
                    //will only evaluate the lines related to the parcels that are selected
                    //selectionSet = CFLineLayer.FeatureClass.Select(new QueryFilter() { WhereClause = "1=0" }, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, null);

                    //will union the current line selection and the parcel selection
                    relatedSelectionSet = CFLineFeatureSelection.SelectionSet.Select(null, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, null);
                        
                    while ((row = (IRow)lineSet.Next()) != null)
                    {
                        relatedSelectionSet.Add(row.OID);
                        Marshal.ReleaseComObject(row);
                    }
                    Marshal.ReleaseComObject(lineSet);
                    polySet.Reset();
                    while ((row = (IRow)polySet.Next()) != null)
                        Marshal.ReleaseComObject(row);
                    Marshal.ReleaseComObject(polySet);

                    selectionSet = relatedSelectionSet;
                }
                else if (CFLineFeatureSelection.SelectionSet.Count > 0)
                {
                    //lines selected
                    selectionSet = CFLineFeatureSelection.SelectionSet;
                }
                    
                else
                {
                    if (DialogResult.OK != messageBox.Show("You are about to run the add-in on the entire feature class, this could take a long time.  Proceeed?", "Long Operation", MessageBoxButtons.OKCancel))
                        return;
                }

                    
                FindCurves(CFLineLayer.Name, CFLineLayer.FeatureClass, selectionSet, whereClause, idxRADIUS, idxCENTERPTID, idxParcelIDFld, progressor);
                if (Curves.Count == 0)
                {
                    messageBox.Show("No inferred curved lines found.");
                    return;
                }
            }
            catch (Exception Exx)
            {
                messageBox.Show(Exx.Message);

                if (relationshipClass != null)
                    Marshal.ReleaseComObject(relationshipClass);
                if (relatedSelectionSet != null)
                    Marshal.ReleaseComObject(relatedSelectionSet);
            }
            finally
            {
                progressor.Dispose();
                Finished = true;
            }
        }
        public void UpdateCurves(IEnumerable<InferredCurve> updateCurves = null)
        {
            if (updateCurves == null)
                updateCurves = Curves;


            if (m_pEd.EditState == esriEditState.esriStateNotEditing)
            {
                messageBox.Show("Please start editing first, and try again.");
                return;
            }

            if (hasOpenJob())
                return;

            IMap pMap = m_pEd.Map;

            myAOProgressor progressor = new myAOProgressor();
            try
            {
                Finished = false;
                progressor.setStepProgressorProperties(1, "Initializing");
                ICadastralFabricLayer CFLayer = GetFabricLayer(pMap);
                ICadastralFabric pCadFabric = CFLayer.CadastralFabric;

                IFeatureLayer CFLineLayer = CFLayer.get_CadastralSubLayer(esriCadastralFabricRenderer.esriCFRLines);
                IFeatureLayer CFParcelLayer = CFLayer.get_CadastralSubLayer(esriCadastralFabricRenderer.esriCFRParcels);

                if (updateCurves.Count() > 0)
                    UpdateCurves(pCadFabric, CFLineLayer.FeatureClass, updateCurves, progressor);
            }
            catch (Exception Exx)
            {
                messageBox.Show(Exx.Message);

                if (m_pEd != null && m_pEd.HasEdits())
                {
                    m_pEd.AbortOperation();
                }
            }
            finally
            {
                progressor.Dispose();
                Finished = true;
            }
        }

        public void FindCurves(String Name, IFeatureClass pFabricLinesFC, ISelectionSet selSet, string whereClause, int idxRADIUS, int idxCENTERPTID, int idxParcelIDFld, myProgessor progressor)
        {
            IQueryFilter qFilter = new QueryFilter();
            if (String.IsNullOrEmpty(whereClause))
            {
                qFilter.WhereClause = "CenterPointID is null and Radius is null";
            }
            else
            {
                qFilter.WhereClause = string.Concat(whereClause, " and CenterPointID is null and Radius is null");
            }

            IFeatureCursor cursor = null;
            if (selSet != null)
            {
                ICursor c;
                ISelectionSet subset = selSet.Select(qFilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, null);
                progressor.setStepProgressorProperties(subset.Count, String.Format("Layer {0}: Evaluating Selected Features", Name));

                subset.Search(null, true, out c);
                cursor = (IFeatureCursor)c;
            }
            else
            {
                progressor.setStepProgressorProperties(((ITable)pFabricLinesFC).RowCount(qFilter), String.Format("Layer {0}: Evaluating Features", Name));
                cursor = pFabricLinesFC.Search(qFilter, true);
            }

            //ISelectionSet pSelSet = pFabricLinesFC.Select(qFilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, null);
            //progressor.setStepProgressorProperties(pSelSet.Count, String.Format("Layer {0}: Evaluating Features", Name));
            //ICursor cursor = null;
            //pSelSet.Search(null, false, out cursor);


            IFeature pLineFeat = null;
            while ((pLineFeat = (IFeature)cursor.NextFeature() as IFeature) != null)
            {
                if (!progressor.Continue())
                    break;
                progressor.Step();

                if (!Curves.Any(w => w.ObjectID == pLineFeat.OID))
                {
                    IGeometry pGeom = pLineFeat.ShapeCopy;
                    ISegmentCollection pSegColl = pGeom as ISegmentCollection;
                    ISegment pSeg = null;
                    if (pSegColl.SegmentCount == 1)
                        pSeg = pSegColl.get_Segment(0);
                    else
                    {
                        //todo: but for now, only deals with single segment short segments
                        Marshal.ReleaseComObject(pLineFeat);
                        continue;
                    }

                    //if the geometry is a circular arc and the attributes reflect that, move on to the next feature
                    //obsolete, filter is pushed to database
                    //if (pSeg is ICircularArc)
                    //{
                    //    object dVal1 = pLineFeat.get_Value(idxRADIUS);
                    //    object dVal2 = pLineFeat.get_Value(idxCENTERPTID);
                    //    if (!(dVal1 is DBNull) && !(dVal2 is DBNull))
                    //    {
                    //        Marshal.ReleaseComObject(pLineFeat);
                    //        continue;
                    //    }
                    //}

                    //query near lines
                    List<RelatedLine> tangentLines;
                    List<RelatedCurve> sCurveInfoFromNeighbours = GetTangentCurveMatchFeatures(pFabricLinesFC, pLineFeat, (IPolycurve)pGeom, "", idxRADIUS, idxCENTERPTID, pSeg.Length, out tangentLines);
                                
                    if(sCurveInfoFromNeighbours.Count > 0)
                    //if (HasTangentCurveMatchFeatures(pFabricLinesFC, (IPolycurve)pGeom, "", pSeg.Length, out iFoundTangent, ref sCurveInfoFromNeighbours))
                    {
                        InferredCurve curve = new InferredCurve(pLineFeat.OID, Name, sCurveInfoFromNeighbours);
                        Curves.Add(curve);
                        curve.PropertyChanged += new PropertyChangedEventHandler(curve_PropertyChanged);

                        if (curve.TangentCurves[0].Orientation == RelativeOrientation.Same || curve.TangentCurves[0].Orientation == RelativeOrientation.Reverse)
                        {
                            curve.InferredRadius = curve.TangentCurves[0].Radius;
                            curve.InferredCenterpointID = curve.TangentCurves[0].CenterpointID;
                        }
                        else
                        {
                            //if there's only one tangent look further afield
                            if (curve.TangentCurves.Count == 1)
                                curve.ParallelCurves = GetParallelCurveMatchFeatures(pFabricLinesFC, (IPolycurve)pGeom, "");
                            
                            //Determin if there is enough information to actually set the radius and centerpoint
                            RefineToBestRadiusAndCenterPoint(curve);

                            if (!curve.HasValue && tangentLines.Count > 0)
                            {
                                IPolyline polyLine = (IPolyline)pGeom;
                                curve.FromPoint = polyLine.FromPoint;
                                curve.ToPoint = polyLine.ToPoint;
                                curve.TangentLines = tangentLines;

                                RefineToBestRadiusAndCenterPoint(curve);
                            }
                        }

                        //if the curve has an accepted curve (ie, it's a candidate to be changed), record the parcel id
                        //if (curve.Accepted != null)
                        //    affectedParcels.Add();

                        //cache the parcel so it can be looked up later
                        curve.Parcel = (int)pLineFeat.get_Value(idxParcelIDFld);
                    }
                }
                Marshal.ReleaseComObject(pLineFeat);
            }
            Marshal.ReleaseComObject(cursor);

            Total = Curves.Count;
            Inferred = Curves.Count(w => w.Action == UpdateAction.Update);
        }
        
        void UpdateCurves(ICadastralFabric pCadFabric, IFeatureClass pFabricLinesFC, IEnumerable<InferredCurve> curvesToUpdate, myProgessor progressor)
        {
            List<InferredCurve> updateCurves = (from InferredCurve c in Curves where c.Action==UpdateAction.Update select c).ToList();

            bool bIsFileBasedGDB = false;
            bool bIsUnVersioned = false;
            bool bUseNonVersionedDelete = false;
            IWorkspace pWS = m_pEd.EditWorkspace;

            if (!SetupEditEnvironment(pWS, pCadFabric, m_pEd, out bIsFileBasedGDB, out bIsUnVersioned, out bUseNonVersionedDelete))
            {
                messageBox.Show("The editing environment could not be initialized");
                return;
            }

            #region Create Cadastral Job
            string sTime = "";
            if (!bIsUnVersioned && !bIsFileBasedGDB)
            {
                //see if parcel locks can be obtained on the selected parcels. First create a job.
                DateTime localNow = DateTime.Now;
                sTime = Convert.ToString(localNow);
                ICadastralJob pJob = new CadastralJob();
                pJob.Name = sTime;
                pJob.Owner = System.Windows.Forms.SystemInformation.UserName;
                pJob.Description = "Convert lines to curves";
                try
                {
                    Int32 jobId = pCadFabric.CreateJob(pJob);
                }
                catch (COMException ex)
                {
                    if (ex.ErrorCode == (int)fdoError.FDO_E_CADASTRAL_FABRIC_JOB_ALREADY_EXISTS)
                    {
                        messageBox.Show("Job named: '" + pJob.Name + "', already exists");
                    }
                    else
                    {
                        messageBox.Show(ex.Message);
                    }
                    return;
                }
            }
            #endregion

            #region Test for Edit Locks
            ICadastralFabricLocks pFabLocks = (ICadastralFabricLocks)pCadFabric;

            //only need to get locks for parcels that have lines that are to be changed

            ILongArray affectedParcels = new LongArrayClass();
            foreach (int i in updateCurves.Select(w=>w.Parcel).Distinct())
                affectedParcels.Add(i);

            if (!bIsUnVersioned && !bIsFileBasedGDB)
            {
                pFabLocks.LockingJob = sTime;
                ILongArray pLocksInConflict = null;
                ILongArray pSoftLcksInConflict = null;

                if (!bIsFileBasedGDB)
                    progressor.setStepProgressorProperties(0, "Testing for edit locks on parcels...");

                try
                {
                    pFabLocks.AcquireLocks(affectedParcels, true, ref pLocksInConflict, ref pSoftLcksInConflict);
                }
                catch (COMException pCOMEx)
                {
                    if (pCOMEx.ErrorCode == (int)fdoError.FDO_E_CADASTRAL_FABRIC_JOB_LOCK_ALREADY_EXISTS ||
                        pCOMEx.ErrorCode == (int)fdoError.FDO_E_CADASTRAL_FABRIC_JOB_CURRENTLY_EDITED)
                    {
                        messageBox.Show("Edit Locks could not be acquired on all selected parcels.");
                        // since the operation is being aborted, release any locks that were acquired
                        pFabLocks.UndoLastAcquiredLocks();
                    }
                    else
                        messageBox.Show(pCOMEx.Message + Environment.NewLine + Convert.ToString(pCOMEx.ErrorCode));

                    return;
                }
            }
            #endregion

            if (m_pEd.EditState == esriEditState.esriStateEditing)
            {
                try
                {
                    m_pEd.StartOperation();
                }
                catch
                {
                    m_pEd.AbortOperation();//abort any open edit operations and try again
                    m_pEd.StartOperation();
                }
            }
            if (bUseNonVersionedDelete)
            {
                if (!StartEditing(pWS, bIsUnVersioned))
                {
                    messageBox.Show("Couldn't start an edit session");
                    return;
                }
            }

            ICadastralFabricSchemaEdit2 pSchemaEd = (ICadastralFabricSchemaEdit2)pCadFabric;
            pSchemaEd.ReleaseReadOnlyFields((ITable)pFabricLinesFC, esriCadastralFabricTable.esriCFTLines); //release for edits

            IQueryFilter m_pQF = new QueryFilter();

            // m_pEd.StartOperation();

            //Slice the list into sets that will fit into an in list
            progressor.setStepProgressorProperties(updateCurves.Count, "Updating geometries");
            for (var i = 0; i < updateCurves.Count; i += 995)
            {
                Dictionary<int, InferredCurve> curvesSlice = updateCurves.Skip(i).Take(995).ToDictionary(w => w.ObjectID);
                if (!progressor.Continue())
                    return;
                m_pQF.WhereClause = String.Format("{0} IN ({1})", pFabricLinesFC.OIDFieldName, String.Join(",", (from oid in curvesSlice.Keys select oid.ToString()).ToArray()));
                UpdateCircularArcValues((ITable)pFabricLinesFC, m_pQF, bIsUnVersioned, curvesSlice, progressor);
            }

            //List<string> sInClauseList = InClauseFromOIDsList(Curves, 995);
            //foreach (string InClause in sInClauseList)
            //{
            //    if (!cancelTracker.Continue())
            //        return;

            //    m_pQF.WhereClause = pFabricLinesFC.OIDFieldName + " IN (" + InClause + ")";
            //    if (!UpdateCircularArcValues((ITable)pFabricLinesFC, m_pQF, bIsUnVersioned, Curves, stepProgress, cancelTracker))
            //        ;
            //}
            m_pEd.StopOperation("Insert missing circular arc information.");
        }

        #region Access

        private ICadastralFabricLayer GetFabricLayer(IMap Map)
        {
            List<ICadastralFabricLayer> ret = new List<ICadastralFabricLayer>();

            IEnumLayer pEnumLayer = Map.get_Layers(null, true);
            ILayer pLayer;
            while ((pLayer = pEnumLayer.Next()) != null)
            {
                ICadastralFabricLayer pCFLayer = pLayer as ICadastralFabricLayer;
                if (pCFLayer != null)
                    ret.Add(pCFLayer);
            }

            if (ret.Count == 0)
            {
                messageBox.Show("No cadastral fabric layers found in the current map.");
                return null;
            }
            if (ret.Count > 1)
            {
                messageBox.Show("Multiple cadastral fabric layers found in the current map.");
                return null;
            }
            return ret[0];
        }

        bool hasOpenJob()
        {
            //get Cadastral Editor
            UID pUID = new UIDClass();
            pUID.Value = "{114D685F-99B7-4B63-B09F-6D1A41A4DDC1}";
            ICadastralExtensionManager2 pCadExtMan = (ICadastralExtensionManager2)ArcMap.Application.FindExtensionByCLSID(pUID);
            ICadastralEditor pCadEd = (ICadastralEditor)ArcMap.Application.FindExtensionByCLSID(pUID);

            //check if there is a Manual Mode "modify" job active ===========
            ICadastralPacketManager pCadPacMan = (ICadastralPacketManager)pCadExtMan;
            if (pCadPacMan.PacketOpen)
            {
                messageBox.Show("The command cannot be used when there is an open job.\r\nPlease finish or discard the open job, and try again.");
                return true;
            }
            return false;
        }

        #endregion

        #region feature updating

        public bool SetupEditEnvironment(IWorkspace TheWorkspace, ICadastralFabric TheFabric, IEditor TheEditor, out bool IsFileBasedGDB, out bool IsUnVersioned, out bool UseNonVersionedEdit)
        {
            IsFileBasedGDB = false;
            IsUnVersioned = false;
            UseNonVersionedEdit = false;

            ITable pTable = TheFabric.get_CadastralTable(esriCadastralFabricTable.esriCFTParcels);

            IsFileBasedGDB = (!(TheWorkspace.WorkspaceFactory.WorkspaceType == esriWorkspaceType.esriRemoteDatabaseWorkspace));

            if (!(IsFileBasedGDB))
            {
                IVersionedObject pVersObj = (IVersionedObject)pTable;
                IsUnVersioned = (!(pVersObj.IsRegisteredAsVersioned));
                pTable = null;
                pVersObj = null;
            }
            if (IsUnVersioned && !IsFileBasedGDB)
            {//
                DialogResult dlgRes = messageBox.Show("Fabric is not registered as versioned." +
                  "\r\n You will not be able to undo." +
                  "\r\n Click 'OK' to delete permanently.",
                  "Continue with delete?", MessageBoxButtons.OKCancel);
                if (dlgRes == DialogResult.OK)
                {
                    UseNonVersionedEdit = true;
                }
                else if (dlgRes == DialogResult.Cancel)
                {
                    return false;
                }
                //MessageBox.Show("The fabric tables are non-versioned." +
                //   "\r\n Please register as versioned, and try again.");
                //return false;
            }
            else if ((TheEditor.EditState == esriEditState.esriStateNotEditing))
            {
                messageBox.Show("Please start editing first and try again.", "Delete",
                  MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        //public void FIDsetToLongArray(IFIDSet InFIDSet, ref ILongArray OutLongArray, ref int[] OutIntArray)
        //{
        //    setStepProgressorProperties(InFIDSet.Count(), "Converting ObjectIDs");

        //    Int32 pfID = -1;
        //    InFIDSet.Reset();
        //    double dMax = InFIDSet.Count();
        //    int iMax = (int)(dMax);
        //    for (Int32 pCnt = 0; pCnt <= (InFIDSet.Count() - 1); pCnt++)
        //    {
        //        InFIDSet.Next(out pfID);
        //        OutLongArray.Add(pfID);
        //        OutIntArray[pCnt] = pfID;
        //        if (stepProgress != null)
        //        {
        //            if (stepProgress.Position < stepProgress.MaxRange)
        //                stepProgress.Step();
        //        }
        //    }
        //    return;
        //}

        public bool StartEditing(IWorkspace TheWorkspace, bool IsUnversioned)   // Start EditSession + create EditOperation
        {
            bool IsFileBasedGDB =
              (!(TheWorkspace.WorkspaceFactory.WorkspaceType == esriWorkspaceType.esriRemoteDatabaseWorkspace));

            IWorkspaceEdit pWSEdit = (IWorkspaceEdit)TheWorkspace;
            if (pWSEdit.IsBeingEdited())
            {
                messageBox.Show("The workspace is being edited by another process.");
                return false;
            }

            if (!IsFileBasedGDB)
            {
                IMultiuserWorkspaceEdit pMUWSEdit = (IMultiuserWorkspaceEdit)TheWorkspace;
                try
                {
                    if (pMUWSEdit.SupportsMultiuserEditSessionMode(esriMultiuserEditSessionMode.esriMESMNonVersioned) && IsUnversioned)
                    {
                        pMUWSEdit.StartMultiuserEditing(esriMultiuserEditSessionMode.esriMESMNonVersioned);
                    }
                    else if (pMUWSEdit.SupportsMultiuserEditSessionMode(esriMultiuserEditSessionMode.esriMESMVersioned) && !IsUnversioned)
                    {
                        pMUWSEdit.StartMultiuserEditing(esriMultiuserEditSessionMode.esriMESMVersioned);
                    }

                    else
                    {
                        return false;
                    }
                }
                catch (COMException ex)
                {
                    messageBox.Show(ex.Message + "  " + Convert.ToString(ex.ErrorCode), "Start Editing");
                    return false;
                }
            }
            else
            {
                try
                {
                    pWSEdit.StartEditing(false);
                }
                catch (COMException ex)
                {
                    messageBox.Show(ex.Message + "  " + Convert.ToString(ex.ErrorCode), "Start Editing");
                    return false;
                }
            }

            pWSEdit.DisableUndoRedo();
            try
            {
                pWSEdit.StartEditOperation();
            }
            catch
            {
                pWSEdit.StopEditing(false);
                return false;
            }
            return true;
        }

        public List<string> InClauseFromOIDsList(IList<InferredCurve> ListOfOids, int TokenMax)
        {
            List<string> InClause = new List<string>();
            int iCnt = 0;
            int iIdx = 0;
            InClause.Add("");
            foreach (InferredCurve i in ListOfOids)
            {
                if (iCnt == TokenMax)
                {
                    InClause.Add("");
                    iCnt = 0;
                    iIdx++;
                }
                if (InClause[iIdx].Trim() == "")
                    InClause[iIdx] = i.ObjectID.ToString();
                else
                    InClause[iIdx] += "," + i.ObjectID.ToString();
                iCnt++;
            }
            return InClause;
        }

        public bool UpdateCircularArcValues(ITable LineTable, IQueryFilter QueryFilter, bool Unversioned, IDictionary<int, InferredCurve> CurveLookup, myProgessor progressor)
        {
            try
            {
                ITableWrite pTableWr = (ITableWrite)LineTable;//used for unversioned table
                IRow pLineFeat = null;
                ICursor pLineCurs = null;

                if (Unversioned)
                    pLineCurs = pTableWr.UpdateRows(QueryFilter, false);
                else
                    pLineCurs = LineTable.Update(QueryFilter, false);

                pLineFeat = null;

                Int32 iCtrPointIDX = pLineCurs.Fields.FindField("CENTERPOINTID");
                Int32 iRadiusIDX = pLineCurs.Fields.FindField("RADIUS");

                while ((pLineFeat = pLineCurs.NextRow()) != null)
                {//loop through all of the given lines, and update centerpoint ids and the Radius values
                    if (!progressor.Continue())
                        return false;
                    progressor.Step();

                    //List<string> CurveInfoList = CurveLookup[pLineFeat.OID];
                    //InferredCurve CurveInfoList = CurveLookup[pLineFeat.OID];

                    //string[] sCurveInfo = CurveInfoList[0].Split(','); //should only be one element in the list at this point
                    //RelatedCurve curveInfo = CurveInfoList.FirstOrDefault(w=>w.Accepted);
                    InferredCurve curveInfo = CurveLookup[pLineFeat.OID];
                    //if (sCurveInfo.Length > 2)
                    //if (!curveInfo.)
                    //{
                    //    Marshal.ReleaseComObject(pLineFeat); //garbage collection
                    //    continue;
                    //}
                    //double dRadius = Convert.ToDouble(sCurveInfo[0]);
                    //int iCtrPointID = Convert.ToInt32(sCurveInfo[1]);

                    pLineFeat.set_Value(iRadiusIDX, curveInfo.InferredRadius);
                    pLineFeat.set_Value(iCtrPointIDX, curveInfo.InferredCenterpointID);

                    if (Unversioned)
                        pLineCurs.UpdateRow(pLineFeat);
                    else
                        pLineFeat.Store();

                    Marshal.ReleaseComObject(pLineFeat); //garbage collection
                }
                Marshal.ReleaseComObject(pLineCurs); //garbage collection
                return true;
            }
            catch (COMException ex)
            {
                messageBox.Show("Problem updating circular arc: " + Convert.ToString(ex.ErrorCode));
                return false;
            }
        }

        #endregion

        #region Inference

        RelatedCurveComparer relatedCurveComparer = new RelatedCurveComparer();
        bool evaluateParallelCurves(InferredCurve inferredCurve, RelatedCurve curve)
        {
            bool bHasConfirmer = false;
            foreach (RelatedCurve dd in inferredCurve.ParallelCurves)
            {
                if (Math.Abs(dd.Radius - curve.Radius) < 0.5)
                {
                    bHasConfirmer = true;
                    break;
                }
            }
            if (bHasConfirmer)
            {
                inferredCurve.InferredRadius = inferredCurve.TangentCurves[0].Radius;
                inferredCurve.InferredCenterpointID = inferredCurve.TangentCurves[0].CenterpointID;
                return true;
            }
            return false;
        }
        bool evaluateTrangent(InferredCurve inferredCurve, RelatedCurve curve)
        {
            //double angle = Math.Atan((inferredCurve.FromPoint.Y - inferredCurve.ToPoint.Y) / (inferredCurve.FromPoint.X - inferredCurve.ToPoint.X));
            //double length = ((IProximityOperator)inferredCurve.FromPoint).ReturnDistance(inferredCurve.ToPoint);
            
            ILine line = new Line() { FromPoint = inferredCurve.FromPoint, ToPoint = inferredCurve.ToPoint };
            double halfdelta = Math.Asin(line.Length / 2 / curve.Radius);
            double newAngle = line.Angle + halfdelta;

            bool bHasConfirmer = false;
            foreach (RelatedLine tangent in inferredCurve.TangentLines)
            {
                if (Math.Abs(tangent.Angle - newAngle) < 0.005)
                {
                    bHasConfirmer = true;
                    break;
                }
            }
            if (bHasConfirmer)
            {
                inferredCurve.InferredRadius = inferredCurve.TangentCurves[0].Radius;
                inferredCurve.InferredCenterpointID = inferredCurve.TangentCurves[0].CenterpointID;
                return true;
            }
            return false;
        }
        void RefineToBestRadiusAndCenterPoint(InferredCurve inferredCurve)
        {
            //only one radius found
            if (inferredCurve.TangentCurves.Count == 1)
            {
                //search the parallel offsets for one conformer, if found return
                if (inferredCurve.ParallelCurves.Count > 0 && evaluateParallelCurves(inferredCurve, inferredCurve.TangentCurves[0]))
                {
                    return;
                }
                //search the tagent lines for one conformer, if found return
                if (inferredCurve.TangentLines.Count > 0 && evaluateTrangent(inferredCurve, inferredCurve.TangentCurves[0]))
                {
                    return;
                }
            }
            
            var groupsTangent = inferredCurve.TangentCurves.GroupBy(item => Math.Round(item.Radius, 2)).Where(group => group.Skip(1).Any());
            var groupsTangentAndCP = inferredCurve.TangentCurves.GroupBy(item => item, relatedCurveComparer).Where(group => group.Skip(1).Any());
            
            //System.Diagnostics.Debug.Print(groupsTangent.Count().ToString());
            //System.Diagnostics.Debug.Print(groupsTangentAndCP.Count().ToString());

            bool HasStartTangents = inferredCurve.TangentCurves.Any(w => w.Orientation == RelativeOrientation.To_From || w.Orientation == RelativeOrientation.To_To);
            bool HasEndTangents = inferredCurve.TangentCurves.Any(w => w.Orientation == RelativeOrientation.From_To || w.Orientation == RelativeOrientation.From_From);

            //if there is only 1 of each group, then there are no ambiguities for the tangent or the center point
            if (groupsTangent.Count() == 1 && groupsTangentAndCP.Count() == 1)
            {
                IGrouping<RelatedCurve, RelatedCurve> d1 = groupsTangentAndCP.ElementAt(0);
                
                //if there are curves on either side of the query feature
                if (HasStartTangents && HasEndTangents)
                {

                    inferredCurve.InferredRadius = d1.Key.Radius;
                    inferredCurve.InferredCenterpointID = d1.Key.CenterpointID;
                    return;
                }
                if (inferredCurve.TangentCurves.Count > 0)
                {
                    //search the parallel offsets for one conformer, if found return
                    if (inferredCurve.ParallelCurves.Count > 0 && evaluateParallelCurves(inferredCurve, d1.Key))
                    {
                        return;
                    }
                    //search the tagent lines for one conformer, if found return
                    if (inferredCurve.TangentLines.Count > 0 && evaluateTrangent(inferredCurve, d1.Key))
                    {
                        return;
                    }
                }
            }
            else if (groupsTangent.Count() == 1 && groupsTangentAndCP.Count() > 1)
            {//if there is only 1 tangent, but more than one center point then there are center points to merge
                
                //TODO: Add center point Merging here
            }
            if (groupsTangent.Count() > 1)
            { //if there is more than 1 tangent, then ...code stub if needed
                foreach (var value in groupsTangentAndCP)
                {
                    System.Diagnostics.Debug.Print(value.Key.ToString());
                }
            }
        }

        public List<RelatedCurve> GetParallelCurveMatchFeatures(IFeatureClass FeatureClass, IPolycurve inPolycurve, string WhereClause)
            //double AngleToleranceTangentCompareInDegrees, double OrthogonalSearchDistance,
            //                                        out int outFoundLinesCount, out int outFoundParallelCurvesCount, ref List<RelatedCurve> CurveInfoFromNeighbours)
        {
            List<RelatedCurve> CurveInfoFromNeighbours = new List<RelatedCurve>();

            ILine inGeomChord = (ILine)new Line();
            inGeomChord.PutCoords(inPolycurve.FromPoint, inPolycurve.ToPoint);
            IVector3D inGeomVector = (IVector3D)new Vector3D();
            inGeomVector.PolarSet(inGeomChord.Angle, 0, 1);

            int idxRadius = FeatureClass.FindField("RADIUS");
            if (idxRadius == -1)
                return CurveInfoFromNeighbours;

            int idxCenterPointID = FeatureClass.FindField("CENTERPOINTID");
            if (idxCenterPointID == -1)
                return CurveInfoFromNeighbours;

            //generate line segments that are perpendicular to the in feature at half way

            IGeometryBag queryGeomBag = (IGeometryBag)new GeometryBag();
            IGeometryCollection queryGeomPartCollection = (IGeometryCollection)queryGeomBag;

            IGeometry queryMultiPartPolyLine = (IGeometry)new Polyline(); //qi
            IGeoDataset pGeoDS = (IGeoDataset)FeatureClass;
            ISpatialReference spatialRef = pGeoDS.SpatialReference;
            queryMultiPartPolyLine.SpatialReference = spatialRef;

            IGeometryCollection queryGeomCollection = queryMultiPartPolyLine as IGeometryCollection;

            ILine pNormalLine = (ILine)new Line(); //new
            for (int i = -1; i < 2; i = i + 2)
            {
                double dOffset = CurveByInferenceSettings.Instance.OrthogonalSearchDistance * i;

                inPolycurve.QueryNormal(esriSegmentExtension.esriNoExtension, 0.5, true, dOffset, pNormalLine);
                ILine pThisLine = (ILine)new Line();

                pThisLine.PutCoords(pNormalLine.FromPoint, pNormalLine.ToPoint);
                queryGeomPartCollection.AddGeometry(pThisLine);

                //Although each line is connected to the other, create a new path for each line 
                //this allows for future changes in case the input needs to be altered to separate paths.

                ISegmentCollection newPath = (ISegmentCollection)new Path();
                object obj = Type.Missing;
                newPath.AddSegment((ISegment)pThisLine, ref obj, ref obj);
                //The spatial reference associated with geometryCollection will be assigned to all incoming paths and segments.
                queryGeomCollection.AddGeometry(newPath as IGeometry, ref obj, ref obj);
            }

            //search for records that intersect these perpendicular geometries

            ISpatialFilter spatialFilter = (ISpatialFilter)new SpatialFilter();
            //spatialFilter.WhereClause = WhereClause;
            if (String.IsNullOrEmpty(WhereClause))
                spatialFilter.WhereClause = "CenterPointID is not null and Radius is not null and Radius <> 0";
            else
                spatialFilter.WhereClause = string.Concat(WhereClause, " and CenterPointID is not null and Radius is not null and Radius <> 0");
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            spatialFilter.SearchOrder = esriSearchOrder.esriSearchOrderSpatial;

            spatialFilter.Geometry = queryGeomBag;

            IFeatureCursor pFeatCursLines = null;
            try
            {
                pFeatCursLines = FeatureClass.Search(spatialFilter, false);
            }
            catch (Exception ex)
            {
                messageBox.Show(ex.Message);
                return CurveInfoFromNeighbours;
            }

            IFeature pFeat = pFeatCursLines.NextFeature();
            while (pFeat != null)
            {
                IGeometry pFoundLineGeom = pFeat.ShapeCopy;

                //if the feature has no radius attribute, skip.
                double dRadius = pFeat.get_Value(idxRadius) is DBNull ? 0 : (double)pFeat.get_Value(idxRadius);
                if (dRadius == 0)
                {//null or zero radius so skip.
                    Marshal.ReleaseComObject(pFeat);
                    pFeat = pFeatCursLines.NextFeature();
                    continue;
                }

                int? centerpointID = pFeat.get_Value(idxCenterPointID) is DBNull ? null : (int?)pFeat.get_Value(idxCenterPointID);
                if (centerpointID == null)
                {//null centrpointID so skip.
                    Marshal.ReleaseComObject(pFeat);
                    pFeat = pFeatCursLines.NextFeature();
                    continue;
                }

                ITopologicalOperator6 pTopoOp6 = (ITopologicalOperator6)queryMultiPartPolyLine;
                IGeometry pResultGeom = pTopoOp6.IntersectEx(pFoundLineGeom, false, esriGeometryDimension.esriGeometry0Dimension);
                if (pResultGeom == null || pResultGeom.IsEmpty)
                {
                    Marshal.ReleaseComObject(pFeat);
                    pFeat = pFeatCursLines.NextFeature();
                    continue;
                }

                ISegmentCollection pFoundLineGeomSegs = pFoundLineGeom as ISegmentCollection;
                bool bHasCurves = false;
                pFoundLineGeomSegs.HasNonLinearSegments(ref bHasCurves);
                if (!bHasCurves)
                {
                    Marshal.ReleaseComObject(pFeat);
                    pFeat = pFeatCursLines.NextFeature();
                    continue;
                }

                IPointCollection5 PtColl = (IPointCollection5)pResultGeom;
                if (PtColl.PointCount > 1)
                {
                    Marshal.ReleaseComObject(pFeat);
                    pFeat = pFeatCursLines.NextFeature();
                    continue;
                }
                IPolycurve pPolyCurve4Tangent = pFoundLineGeom as IPolycurve;

                for (int j = 0; j < PtColl.PointCount; j++)
                {
                    IPoint p = PtColl.get_Point(j);
                    IPoint outPoint = (IPoint)new Point();
                    double dDistanceAlong = 0;
                    double dDistanceFromCurve = 0;
                    bool bOffsetRight = true;

                    //work out if the point is to the left or right of the original 
                    inPolycurve.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, p, false, outPoint,
                      ref dDistanceAlong, ref dDistanceFromCurve, ref bOffsetRight);

                    ILine pTangent = (ILine)new Line();
                    dDistanceAlong = 0;
                    dDistanceFromCurve = 0;
                    bool bOnRight = true;

                    pPolyCurve4Tangent.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, p, false, outPoint,
                      ref dDistanceAlong, ref dDistanceFromCurve, ref bOnRight);
                    pPolyCurve4Tangent.QueryTangent(esriSegmentExtension.esriNoExtension, dDistanceAlong, false, 100, pTangent);

                    //compare the tangent bearing with the normal to check for orthogonality
                    IVector3D vecTangent = (IVector3D)new Vector3D();
                    vecTangent.PolarSet(pTangent.Angle, 0, 1);

                    IVector3D vecNormal = (IVector3D)new Vector3D();
                    vecNormal.PolarSet(pNormalLine.Angle, 0, 1);

                    ILine pHitDistanceForRadiusDifference = (ILine)new Line();
                    pHitDistanceForRadiusDifference.PutCoords(pNormalLine.FromPoint, outPoint);
                    double dRadiusDiff = pHitDistanceForRadiusDifference.Length;

                    double dDotProd = vecTangent.DotProduct(vecNormal);
                    double dAngleCheck = Math.Acos(dDotProd) * 180 / Math.PI; //in degrees
                    dAngleCheck = Math.Abs(dAngleCheck - 90);

                    if (dAngleCheck < CurveByInferenceSettings.Instance.AngleToleranceTangentCompareInDegrees)
                    {
                        //work out concavity orientation with respect to the original line using the radius sign and dot product
                        dDotProd = inGeomVector.DotProduct(vecTangent);
                        double dTangentCheck = Math.Acos(dDotProd) * 180 / Math.PI; // in degrees
                        //dTangentCheck at this point should be close to 0 or 180 degrees.
                        
                        bool bIsConvex = ((dTangentCheck < 90 && dRadius < 0 && !bOffsetRight) ||
                                          (dTangentCheck > 90 && dRadius > 0 && !bOffsetRight) ||
                                          (dTangentCheck < 90 && dRadius > 0 && bOffsetRight) ||
                                          (dTangentCheck > 90 && dRadius < 0 && bOffsetRight));

                        double dUnitSignChange = 1;

                        if (!bIsConvex)
                            dUnitSignChange = -1;

                        double dDerivedRadius = (Math.Abs(dRadius)) + dRadiusDiff * dUnitSignChange;

                        dUnitSignChange = 1;
                        //now compute inferred left/right for candidate
                        if (bIsConvex && !bOffsetRight)
                            dUnitSignChange = -1;

                        if (!bIsConvex && bOffsetRight)
                            dUnitSignChange = -1;

                        dDerivedRadius = dDerivedRadius * dUnitSignChange;

                        //string sHarvestedCurveInfo = pFeat.OID.ToString() + "," + dDerivedRadius.ToString("#.000") + "," + centerpointID.ToString() + "," + dRadiusDiff.ToString("#.000");
                        CurveInfoFromNeighbours.Add(new RelatedCurve(pFeat.OID, dDerivedRadius, centerpointID.Value, RelativeOrientation.Parallel));
                    }
                }

                Marshal.ReleaseComObject(pFeat);
                pFeat = pFeatCursLines.NextFeature();
            }
            Marshal.FinalReleaseComObject(pFeatCursLines);

            return CurveInfoFromNeighbours;
        }

        public List<RelatedCurve> GetTangentCurveMatchFeatures(IFeatureClass FeatureClass, IFeature inFeature, IPolycurve inPolycurve, string WhereClause,
                                                int idxRadius, int idxCenterPointID,
                                                double SegementLength, out List<RelatedLine> tangentLines)
            //double AngleToleranceTangentCompareInDegrees, double StraightLinesBreakLessThanInDegrees, 
            //double MaximumDeltaInDegrees, double ExcludeTangentsShorterThan,
            //                                     out int outFoundTangentCurvesCount, ref List<RelatedCurve> CurveInfoFromNeighbours)
        {
            List<RelatedCurve> CurveInfoFromNeighbours = new List<RelatedCurve>();
            tangentLines = new List<RelatedLine>();

            ILine pOriginalChord = (ILine)new Line();
            pOriginalChord.PutCoords(inPolycurve.FromPoint, inPolycurve.ToPoint);
            IVector3D vecOriginalSelected = (IVector3D)new Vector3D();
            vecOriginalSelected.PolarSet(pOriginalChord.Angle, 0, 1);

            IGeometryBag pGeomBag = (IGeometryBag)new GeometryBag();
            IGeometryCollection pGeomColl = (IGeometryCollection)pGeomBag;

            IGeometry MultiPartPolyLine = (IGeometry)new Polyline(); //qi
            IGeoDataset pGeoDS = (IGeoDataset)FeatureClass;
            ISpatialReference spatialRef = pGeoDS.SpatialReference;
            MultiPartPolyLine.SpatialReference = spatialRef;

            IGeometryCollection geometryCollection2 = MultiPartPolyLine as IGeometryCollection;

            ILine inGeomTangentLineAtEnd = new Line(); //new
            ILine inGeomTangentLineAtStart = new Line(); //new
            object objMissing = Type.Missing;

            for (int i = 0; i < 2; i++)
            {
                ILine pThisLine = null;
                if (i == 0)
                {
                    //Add line tangent at end point
                    inPolycurve.QueryTangent(esriSegmentExtension.esriExtendAtTo, 1.0, true, CurveByInferenceSettings.Instance.TangentQueryLength, inGeomTangentLineAtEnd);
                    pThisLine = new Line();
                    pThisLine.PutCoords(inGeomTangentLineAtEnd.FromPoint, inGeomTangentLineAtEnd.ToPoint);
                    pGeomColl.AddGeometry(pThisLine);
                }
                else
                {
                    //Add line tangent at start point
                    inPolycurve.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0.0, true, CurveByInferenceSettings.Instance.TangentQueryLength, inGeomTangentLineAtStart);
                    pThisLine = new Line();
                    pThisLine.PutCoords(inGeomTangentLineAtStart.FromPoint, inGeomTangentLineAtStart.ToPoint);
                    pGeomColl.AddGeometry(pThisLine);
                }
                //Create a new path for each line.

                ISegmentCollection newPath = (ISegmentCollection)new Path();
                newPath.AddSegment((ISegment)pThisLine, ref objMissing, ref objMissing);
                //The spatial reference associated with geometryCollection will be assigned to all incoming paths and segments.
                geometryCollection2.AddGeometry(newPath as IGeometry, ref objMissing, ref objMissing);
            }

            //now buffer the tangent lines
            IGeometryCollection outBufferedGeometryCol = (IGeometryCollection)new GeometryBag();
            for (int jj = 0; jj < geometryCollection2.GeometryCount; jj++)
            {
                IPath pPath = geometryCollection2.get_Geometry(jj) as IPath;
                IGeometryCollection pPolyL = (IGeometryCollection)new Polyline();
                pPolyL.AddGeometry((IGeometry)pPath);

                ITopologicalOperator topologicalOperator = (ITopologicalOperator)pPolyL;
                IPolygon pBuffer = topologicalOperator.Buffer(CurveByInferenceSettings.Instance.TangentQueryBuffer) as IPolygon;
                outBufferedGeometryCol.AddGeometry(pBuffer, ref objMissing, ref objMissing);
            }
            ITopologicalOperator pUnionedBuffers = null;
            pUnionedBuffers = new Polygon() as ITopologicalOperator;
            pUnionedBuffers.ConstructUnion((IEnumGeometry)outBufferedGeometryCol);

            ISpatialFilter pSpatFilt = new SpatialFilter();
            pSpatFilt.WhereClause = WhereClause;
            pSpatFilt.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSpatFilt.SearchOrder = esriSearchOrder.esriSearchOrderSpatial;

            pSpatFilt.Geometry = (IGeometry)pUnionedBuffers;

            IFeatureCursor pFeatCursLines = null;
            try
            {
                pFeatCursLines = FeatureClass.Search(pSpatFilt, false);
            }
            catch (Exception ex)
            {
                messageBox.Show(ex.Message);
                return CurveInfoFromNeighbours;
            }
            IVector3D foundGeomVector = (IVector3D)new Vector3D();
            IFeature foundFeature = null;
            bool bHasTangentStraightLineAtJunction = false;

            //A list of relative orientation values that have large breaks
            List<RelativeOrientation> lstLargeBreak = new List<RelativeOrientation>();

            while ((foundFeature = pFeatCursLines.NextFeature()) != null)
            {
                if (inFeature.OID == foundFeature.OID)
                {
                    Marshal.ReleaseComObject(foundFeature);
                    continue;
                }

                IGeometry foundLineGeom = foundFeature.ShapeCopy;
                IPolycurve foundPolyCurve = foundLineGeom as IPolycurve;
                RelativeOrientation iRelativeOrientation = GetRelativeOrientation(foundPolyCurve, inPolycurve);
                //iRelativeOrientation == 1 --> closest points are original TO and found TO
                //iRelativeOrientation == 2 --> closest points are original TO and found FROM
                //iRelativeOrientation == 3 --> closest points are original FROM and found TO
                //iRelativeOrientation == 4 --> closest points are original FROM and found FROM

                double foundRadius = (foundFeature.get_Value(idxRadius) is DBNull) ? 0 : (double)foundFeature.get_Value(idxRadius);
                int? foundCentriodID = (foundFeature.get_Value(idxCenterPointID) is DBNull) ? null : (int?)foundFeature.get_Value(idxCenterPointID);

                //if the found feature has the same start and endpoints, assume that the feature is the same.  If that feature has radius and centerpoint information
                //assume that feature is unchanged and update the current feature with the found feature.
                if (iRelativeOrientation == RelativeOrientation.Same || iRelativeOrientation == RelativeOrientation.Reverse)
                {
                    if (foundRadius > 0 && foundCentriodID.HasValue)
                    {
                        double adjustedRadius = iRelativeOrientation == RelativeOrientation.Same ? foundRadius : -1 * foundRadius;

                        CurveInfoFromNeighbours.Clear();
                        CurveInfoFromNeighbours.Add(new RelatedCurve(foundFeature.OID, adjustedRadius, foundCentriodID.Value, iRelativeOrientation));
                        Marshal.ReleaseComObject(foundFeature);
                        Marshal.FinalReleaseComObject(pFeatCursLines);
                        return CurveInfoFromNeighbours;
                    }
                    Marshal.ReleaseComObject(foundFeature);
                    continue;
                }

                /******************************
                 * 
                 *   Compare chord of the in geometry vs the found geometry
                 * 
                 * ****************************/


                ILine foundChord = new Line();
                foundChord.PutCoords(foundPolyCurve.FromPoint, foundPolyCurve.ToPoint);

                //Not sure about this, but it looks like this may be sensitive to the order in which the records are returned

                //first check for likelihood that subject line is supposed to stay straight, by geometry chord bearing angle break test
                //compare the in geometries chord with the found geometries chord
                foundGeomVector.PolarSet(foundChord.Angle, 0, 1);
                double chordsAngleDelta = Math.Abs(Math.Acos(foundGeomVector.DotProduct(vecOriginalSelected)) * 180 / Math.PI); //in degrees

                //large angle break non-tangent, greater than 3 degrees
                if (chordsAngleDelta > CurveByInferenceSettings.Instance.dLargeAngleBreakInDegrees && chordsAngleDelta < (180 - CurveByInferenceSettings.Instance.dLargeAngleBreakInDegrees))
                {
                    if (!lstLargeBreak.Contains(iRelativeOrientation))
                    {
                        lstLargeBreak.Add(iRelativeOrientation);

                        //check to see if there is already a related curve for the current relative orientation
                        //if(CurveInfoFromNeighbours.Any(w=>w.Orientation == iRelativeOrientation))
                        //{
                        //    bHasTangentStraightLineAtJunction = true;
                        //}
                    }

                }

                double ExcludeTangentsShorterThan = SegementLength * CurveByInferenceSettings.Instance.ExcludeTangentsShorterThanScaler;
                
                /*
                if(foundPolyCurve.Length < ExcludeTangentsShorterThan)
                {
                    //exclude short segements
                    Marshal.ReleaseComObject(foundFeature);
                    continue;
                }
                else if (!foundCentriodID.HasValue && foundRadius == 0)
                {
                    //attribute straight line, check the geometry to see if it is tangent at the endpoint
                    if (chordsAngleDelta <= CurveByInferenceSettings.Instance.StraightLinesBreakLessThanInDegrees || (180 - chordsAngleDelta) < CurveByInferenceSettings.Instance.StraightLinesBreakLessThanInDegrees)
                    {
                        if (lstLargeBreak.Contains(iRelativeOrientation))
                            bHasTangentStraightLineAtJunction = true;
                    }
                }
                else if (foundCentriodID.HasValue && foundRadius != 0)
                {
                    //attribute curved line
                    if ((inPolycurve.Length / foundRadius * 180 / Math.PI) > CurveByInferenceSettings.Instance.MaximumDeltaInDegrees)
                    {
                        //if the resulting curve ('in feature' length with 'found feature' radius) would have a central angle more than MaximumDeltaInDegrees degrees then skip
                        Marshal.ReleaseComObject(foundFeature);
                        continue;
                    }
                }
                else
                {
                    //invalid state, curves should have both centriod and radius, straigh lines should have niether.
                    Marshal.ReleaseComObject(foundFeature);
                    throw new Exception(string.Format("Invalid attribute state")); 
                }
                 */

                if ((chordsAngleDelta <= CurveByInferenceSettings.Instance.StraightLinesBreakLessThanInDegrees || (180 - chordsAngleDelta) < CurveByInferenceSettings.Instance.StraightLinesBreakLessThanInDegrees)
                    && !foundCentriodID.HasValue && foundRadius == 0 && !(foundPolyCurve.Length < ExcludeTangentsShorterThan))
                {
                    if (lstLargeBreak.Contains(iRelativeOrientation))
                        bHasTangentStraightLineAtJunction = true;
                }

                if (!foundCentriodID.HasValue || foundRadius == 0 || foundPolyCurve.Length < ExcludeTangentsShorterThan)
                {
                    //staight line
                    if (foundPolyCurve.Length > ExcludeTangentsShorterThan)
                    {
                        IPolyline polyline = (IPolyline4)foundFeature.ShapeCopy;
                        ISegmentCollection collection = (ISegmentCollection)polyline;
                        ISegment segement = collection.get_Segment(collection.SegmentCount - 1);
                        ILine line = new Line();
                        
                        if (iRelativeOrientation == RelativeOrientation.To_From)
                        {
                            polyline.QueryTangent(esriSegmentExtension.esriNoExtension, 0, true, 1.0, line);
                        }
                        if (iRelativeOrientation == RelativeOrientation.From_To)
                        {
                            inPolycurve.QueryTangent(esriSegmentExtension.esriNoExtension, 1, true, 1.0, line);
                        }
                        else if (iRelativeOrientation == RelativeOrientation.To_To)
                        {
                            inPolycurve.QueryTangent(esriSegmentExtension.esriNoExtension, 0, true, 1.0, line);
                        }
                        if (iRelativeOrientation == RelativeOrientation.From_From)
                        {   
                            inPolycurve.QueryTangent(esriSegmentExtension.esriNoExtension, 1, true, 1.0, line);
                        }

                        tangentLines.Add(new RelatedLine(foundFeature.OID, line.Angle, iRelativeOrientation));
                    }
                    //if the feature has a null centrpointID then skip.
                    Marshal.ReleaseComObject(foundFeature);
                    continue;
                }

                if (Math.Abs(inPolycurve.Length / foundRadius * 180 / Math.PI) > CurveByInferenceSettings.Instance.MaximumDeltaInDegrees)
                {
                    //if the resulting curve ('in feature' length with 'found feature' radius) would have a central angle more than MaximumDeltaInDegrees degrees then skip
                    Marshal.ReleaseComObject(foundFeature);
                    continue;
                }

                /******************************
                 * 
                 *   Compare two lines, one from each in geometry and found geometry taken at the closes ends of the lines
                 *   Ie, compare the slopes of the two lines where they touch (or at least where they closest).
                 * 
                 * ****************************/

                //if geometry of curve neighbor curves have been cracked then there can be more than one segment
                //however since all segments would be circular arcs, just need to test the first segment
                ISegmentCollection pFoundLineGeomSegs = foundLineGeom as ISegmentCollection;
                ISegment pSeg = pFoundLineGeomSegs.get_Segment(0);
                if (!(pSeg is ICircularArc))
                {
                    Marshal.ReleaseComObject(foundFeature);
                    continue;
                }

                IVector3D vect1 = (IVector3D)new Vector3D();
                IVector3D vect2 = (IVector3D)new Vector3D();
                ILine tang = new Line();
                double dUnitSignChange = 1;
                if (iRelativeOrientation == RelativeOrientation.To_To) //closest points are original TO and found TO
                {
                    dUnitSignChange = -1;
                    vect1.PolarSet(inGeomTangentLineAtEnd.Angle, 0, 1);
                    foundPolyCurve.QueryTangent(esriSegmentExtension.esriExtendAtTo, 1.0, true, 1, tang);
                    vect2.PolarSet(tang.Angle, 0, 1);
                }
                else if (iRelativeOrientation == RelativeOrientation.To_From)//closest points are original TO and found FROM
                {
                    vect1.PolarSet(inGeomTangentLineAtEnd.Angle, 0, 1);
                    foundPolyCurve.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0.0, true, 1, tang);
                    vect2.PolarSet(tang.Angle, 0, 1);
                }
                else if (iRelativeOrientation == RelativeOrientation.From_To)//closest points are original FROM and found TO
                {
                    vect1.PolarSet(inGeomTangentLineAtStart.Angle, 0, 1);
                    foundPolyCurve.QueryTangent(esriSegmentExtension.esriExtendAtTo, 1.0, true, 1, tang);
                    vect2.PolarSet(tang.Angle, 0, 1);
                }
                else if (iRelativeOrientation == RelativeOrientation.From_From)//closest points are original FROM and found FROM
                {
                    dUnitSignChange = -1;
                    vect1.PolarSet(inGeomTangentLineAtStart.Angle, 0, 1);
                    foundPolyCurve.QueryTangent(esriSegmentExtension.esriExtendAtFrom, 0.0, true, 1, tang);
                    vect2.PolarSet(tang.Angle, 0, 1);
                }

                double tangentAngleDelta = Math.Abs(Math.Acos(vect1.DotProduct(vect2)) * 180 / Math.PI); //in degrees
                if (tangentAngleDelta < CurveByInferenceSettings.Instance.AngleToleranceTangentCompareInDegrees || (180 - tangentAngleDelta) < CurveByInferenceSettings.Instance.AngleToleranceTangentCompareInDegrees)
                {

                    double inferredRadius = foundRadius * dUnitSignChange;

                    //string sHarvestedCurveInfo = foundFeature.OID.ToString() + "," + inferredRadius.ToString("#.000") + "," + foundCentriodID.ToString() + "," + "t";
                    CurveInfoFromNeighbours.Add(new RelatedCurve(foundFeature.OID, inferredRadius, foundCentriodID.Value, iRelativeOrientation));
                }
                Marshal.ReleaseComObject(foundFeature);
            }
            Marshal.FinalReleaseComObject(pFeatCursLines);

            if (bHasTangentStraightLineAtJunction)
                CurveInfoFromNeighbours.Clear(); //open to logic change to be less conservative

            //Original source didn't clear the list before, returing all the found records by reference.
            //calling procedure didn't do anything with those features though, only consumed the features is true was returned

            return CurveInfoFromNeighbours;
        }

        public enum RelativeOrientation { To_To = 1, To_From = 2, From_To = 3, From_From = 4, Same = 5, Reverse = 6, Parallel = 7}
        private RelativeOrientation GetRelativeOrientation(IPolycurve pFoundLineAsPolyCurve, IPolycurve inPolycurve)
        {
            //iRelativeOrientation == 1 --> closest points are original TO and found TO
            //iRelativeOrientation == 2 --> closest points are original TO and found FROM
            //iRelativeOrientation == 3 --> closest points are original FROM and found TO
            //iRelativeOrientation == 4 --> closest points are original FROM and found FROM

            RelativeOrientation ret = RelativeOrientation.To_To;
            double min = 0;

            double To_To = min = ((IProximityOperator)pFoundLineAsPolyCurve.ToPoint).ReturnDistance(inPolycurve.ToPoint);
            double From_From = ((IProximityOperator)pFoundLineAsPolyCurve.FromPoint).ReturnDistance(inPolycurve.FromPoint);

            //Check to see if it is the same line
            if (To_To < 0.005 && From_From < 0.005)
                return (RelativeOrientation.Same);
            
            //short cuts
            if (To_To < 0.005)
                return RelativeOrientation.To_To;
            if (From_From < 0.005)
                return RelativeOrientation.From_From;

            //check to see whats closer
            if (From_From < To_To)
            {
                ret = RelativeOrientation.From_From;
                min = From_From;
            }

            double From_To = ((IProximityOperator)pFoundLineAsPolyCurve.ToPoint).ReturnDistance(inPolycurve.FromPoint);
            double To_From = ((IProximityOperator)pFoundLineAsPolyCurve.FromPoint).ReturnDistance(inPolycurve.ToPoint);

            //Check to see if it is the same line
            if (From_To < 0.005 && To_From < 0.005)
                return (RelativeOrientation.Reverse);

            //short cuts
            if (From_To < 0.005)
                return RelativeOrientation.From_To;
            if (To_From < 0.005)
                return RelativeOrientation.To_From;

            //check to see whats closer
            if (From_To < min)
            {
                ret = RelativeOrientation.From_To;
                min = From_To;
            }
            //check to see whats closer
            if (To_From < min)
            {
                ret = RelativeOrientation.To_From;
            }

            return (RelativeOrientation)ret;
        }
        private RelativeOrientation GetRelativeOrientation2(IPolycurve pFoundLineAsPolyCurve, IPolycurve inPolycurve)
        {
            //iRelativeOrientation == 1 --> closest points are original TO and found TO
            //iRelativeOrientation == 2 --> closest points are original TO and found FROM
            //iRelativeOrientation == 3 --> closest points are original FROM and found TO
            //iRelativeOrientation == 4 --> closest points are original FROM and found FROM
            
            Dictionary<int, double> dictSort2GetShortest = new Dictionary<int, double>();
            dictSort2GetShortest.Add(1, ((IProximityOperator)pFoundLineAsPolyCurve.ToPoint).ReturnDistance(inPolycurve.ToPoint));
            dictSort2GetShortest.Add(2, ((IProximityOperator)pFoundLineAsPolyCurve.FromPoint).ReturnDistance(inPolycurve.ToPoint));
            dictSort2GetShortest.Add(3, ((IProximityOperator)pFoundLineAsPolyCurve.ToPoint).ReturnDistance(inPolycurve.FromPoint));
            dictSort2GetShortest.Add(4, ((IProximityOperator)pFoundLineAsPolyCurve.FromPoint).ReturnDistance(inPolycurve.FromPoint));

            return (RelativeOrientation)dictSort2GetShortest.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
        }
        private RelativeOrientation GetRelativeOrientation_org(IPolycurve pFoundLineAsPolyCurve, IPolycurve inPolycurve)
        {
            //iRelativeOrientation == 1 --> closest points are original TO and found TO
            //iRelativeOrientation == 2 --> closest points are original TO and found FROM
            //iRelativeOrientation == 3 --> closest points are original FROM and found TO
            //iRelativeOrientation == 4 --> closest points are original FROM and found FROM

            Dictionary<int, double> dictSort2GetShortest = new Dictionary<int, double>();

            ILine pFoundTo_2_OriginalTo = new Line();
            pFoundTo_2_OriginalTo.PutCoords(pFoundLineAsPolyCurve.ToPoint, inPolycurve.ToPoint);
            dictSort2GetShortest.Add(1, pFoundTo_2_OriginalTo.Length);

            ILine pFoundFrom_2_OriginalTo = new Line();
            pFoundFrom_2_OriginalTo.PutCoords(pFoundLineAsPolyCurve.FromPoint, inPolycurve.ToPoint);
            dictSort2GetShortest.Add(2, pFoundFrom_2_OriginalTo.Length);

            ILine pFoundTo_2_OriginalFrom = new Line();
            pFoundTo_2_OriginalFrom.PutCoords(pFoundLineAsPolyCurve.ToPoint, inPolycurve.FromPoint);
            dictSort2GetShortest.Add(3, pFoundTo_2_OriginalFrom.Length);

            ILine pFoundFrom_2_OriginalFrom = new Line();
            pFoundFrom_2_OriginalFrom.PutCoords(pFoundLineAsPolyCurve.FromPoint, inPolycurve.FromPoint);
            dictSort2GetShortest.Add(4, pFoundFrom_2_OriginalFrom.Length);

            var sortedDict = from entry in dictSort2GetShortest orderby entry.Value ascending select entry;
            var pEnum = sortedDict.GetEnumerator();
            pEnum.MoveNext(); //get the first key for the shortest line
            var pair = pEnum.Current;
            int iOpt = pair.Key;
            return (RelativeOrientation)iOpt;
        }

        #endregion

        #region UI Update

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(params string[] properties)
        {
            if (PropertyChanged != null)
                foreach (string property in properties)
                    PropertyChanged(this, new PropertyChangedEventArgs(property));

        }

        void curve_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Action":
                    Inferred = Curves.Count(w => w.Action == UpdateAction.Update);
                    break;
            }
        }

        #endregion
    }
}
