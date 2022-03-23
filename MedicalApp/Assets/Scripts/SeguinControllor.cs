using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MedicalApp.Core
{
    public class SeguinControllor : MonoBehaviour
    {
        public static SeguinControllor Instance;
        private const string SEGUIN_OBJECT_TAG = "SeguinObject";

        [Header("Shapes")]
        public SO_SeguinShape[] seguinShapes;
        public GameObject seguinShapePrefab;

        [Header("Rounds")]
        private int maxRoundCount = 3;
        private int currRoundCount = 3;
        public float roundMaxTime = 180f;

        [Header("UI")]
        public GameObject startButton;
        public TMP_Text roundCounter;

        [Header("Data Entry")]
        public SO_DAO dao;

        private Timer timer;
        private List<SeguinSpawnPoint> seguinSpawnPoints;

        private void Awake()
        {
            MakeSureInstanceIsStatic();
        }

        private void OnEnable()
        {
            ObjectDragController.OnDropActionPerformed += HandleDropEvent;
            ObjectDragController.OnDragActionStarted += HandleDragStartEvent;
            Timer.OnTimerEnd += HandleTimerEndEvent;
        }

        private void OnDisable()
        {
            ObjectDragController.OnDropActionPerformed -= HandleDropEvent;
            ObjectDragController.OnDragActionStarted -= HandleDragStartEvent;
            Timer.OnTimerEnd -= HandleTimerEndEvent;
        }

        // Start is called before the first frame update
        void Start()
        {
            timer = GetComponent<Timer>();
            GetSeguinSpawnPoints();
            InstantiateSeguinFormBoard();
            currRoundCount = maxRoundCount;
            ManageRoundCounter();
        }
       
        public void StartRound()
        {
            InitializeTimer();
            MarkObjectsAsInteractable();
            ChangeStartButtonState(false);
        }

        public void ResetCurentRound()
        {
            timer.StopTimer();
            InstantiateSeguinFormBoard();
        }

        public void CancelTest()
        {

        }

        private void InstantiateSeguinFormBoard()
        {
            ManageRoundCounter();
            DestroySeguinObjectsIfAny();
            ResetAllSpawnPoints();
            CreateSeguinObjects();
            ChangeStartButtonState(true);
        }

        private void ChangeStartButtonState(bool enable)
        {
            startButton.SetActive(enable);
        }

        private void ManageRoundCounter()
        {
            roundCounter.text = "Round " + (maxRoundCount - currRoundCount + 1);
        }

        private void MarkObjectsAsInteractable()
        {
            SeguinObject[] objects = FindObjectsOfType<SeguinObject>();

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].isInteractable = true;
            }

        }

        private void DestroySeguinObjectsIfAny()
        {
            GameObject[] toDestroyList = GameObject.FindGameObjectsWithTag(SEGUIN_OBJECT_TAG);
            
            for(int i=0; i<toDestroyList.Length; i++)
            {
                Destroy(toDestroyList[i]);
            }
        }

        private void ResetAllSpawnPoints()
        {
            foreach(SeguinSpawnPoint point in seguinSpawnPoints)
            {
                point.DetachShape();
            }
        }
   
        private void CreateSeguinObjects()
        {
            foreach(SO_SeguinShape shape in seguinShapes)
            {
                GameObject newShape = Instantiate(seguinShapePrefab);
               
                SeguinObject seguinObj = newShape.GetComponent<SeguinObject>();

                Relocate(seguinObj);
                newShape.transform.localScale = new Vector3(
                    shape.scale.x * newShape.transform.localScale.x, 
                    shape.scale.y * newShape.transform.localScale.y, 
                    shape.scale.z * newShape.transform.localScale.z);
                newShape.GetComponent<SpriteRenderer>().sprite = shape.seguinImage;
                seguinObj.objectType = shape.objectType;
                newShape.AddComponent<PolygonCollider2D>();
            }
        }

        private void GetSeguinSpawnPoints()
        {
            seguinSpawnPoints = new List<SeguinSpawnPoint>();
            SeguinSpawnPoint[] points = FindObjectsOfType<SeguinSpawnPoint>();
            seguinSpawnPoints.AddRange(points);
        }

        private SeguinSpawnPoint GetRandomSpawnPoint()
        {
            List<SeguinSpawnPoint> openPoints = new List<SeguinSpawnPoint>();

            foreach (SeguinSpawnPoint point in seguinSpawnPoints)
            {
                if (point.isSeguinObjectAttached) continue;

                openPoints.Add(point);
            }

            int randomIndex = Random.Range(0, openPoints.Count - 1);

            return openPoints[randomIndex];

        }

        private void Relocate(SeguinObject obj)
        {
            SeguinSpawnPoint point = GetRandomSpawnPoint();
            obj.currentRestingSpot = point;
            point.AttachShape(obj.gameObject);
            obj.transform.position = point.transform.position;
        }

        private void HandleDragStartEvent(SeguinObject seguinObject)
        {
            seguinObject.currentRestingSpot.DetachShape();
        }

        private void HandleDropEvent(SeguinObject currentObj, RaycastHit2D[] dropSiteItems)
        {
            if (currentObj == null) return;

            bool relocate = true;

            foreach (RaycastHit2D hit in dropSiteItems)
            {
               
                if (hit.transform.gameObject == currentObj.gameObject) continue;

                if (hit.transform.gameObject.GetComponent<SeguinHole>() != null)
                {
                    SeguinHole hole = hit.transform.gameObject.GetComponent<SeguinHole>();
                    bool isObjectFullyInsideHole = hole.IsInside(hole.GetComponent<PolygonCollider2D>(), currentObj.GetComponent<PolygonCollider2D>());
                    bool isObjectSameAsHole = hole.holeType == currentObj.objectType;
                    if (isObjectFullyInsideHole && isObjectSameAsHole)
                    {
                        relocate = false;
                        Destroy(currentObj);
                    }
                }
            }

            if (relocate)
            {
                Relocate(currentObj);
            }
            else
            {
                CheckIsRoundOver();
            }  
        }

        private void CheckIsRoundOver()
        {
            bool isRoundOver = true;
            foreach(SeguinSpawnPoint point in seguinSpawnPoints)
            {
                if (point.isSeguinObjectAttached)
                {
                    isRoundOver = false;
                    break;
                }
            }

            if (isRoundOver)
            {
                EndRound();
            }
        }

        private void EndRound(bool storeData=true)
        {
            timer.StopTimer();           
            currRoundCount--;
            EnterRoundData();
            if (currRoundCount <= 0)
            {
                FinalCalaculations();
                dao.AppendDataToCsv();

                FindObjectOfType<SceneTransitionHandler>().ChangeScene("FinishTest");
            }
            else
            {
                InstantiateSeguinFormBoard();
            }
        }

        private void HandleTimerEndEvent(TimerDetails timerDetails)
        {
            EndRound();
        }

        private void InitializeTimer()
        {
            timer.StartTimer("roundTimer", roundMaxTime);
        }

        private void EnterRoundData()
        {
            string key = "round_" + (maxRoundCount - currRoundCount) + "_time";
            dao.AddItem(key, timer.GetPassedTime().ToString());
        }

        private void FinalCalaculations()
        {
            /*
            * Min Round
            * Mental Age
            * Chronological Age
            * IQ
            */

            float r1Time = float.Parse(dao.dataMap[SO_DAO.ROUND_1_TIME]);
            float r2Time = float.Parse(dao.dataMap[SO_DAO.ROUND_2_TIME]);
            float r3Time = float.Parse(dao.dataMap[SO_DAO.ROUND_3_TIME]);
            float minRoundSolveTime = Mathf.Min(r1Time, r2Time, r3Time);
            dao.AddItem(SO_DAO.MIN_ROUND_TIME, minRoundSolveTime.ToString());

            int mentalAge = new MentalAgeCalculator().GetMentalAge(minRoundSolveTime);
            dao.AddItem(SO_DAO.MENTAL_AGE, mentalAge.ToString());

            int chronologicalAge = int.Parse(dao.dataMap[SO_DAO.AGE]);
            dao.AddItem(SO_DAO.IQ, new IQCalclulator().GetIq(mentalAge, chronologicalAge).ToString());

            print(dao.dataMap[SO_DAO.MENTAL_AGE]);
            print(dao.dataMap[SO_DAO.IQ]);
        }

        private void MakeSureInstanceIsStatic()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }
}

