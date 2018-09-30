using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLib.Unity
{
    // TODO : Turn this into child of VisualLine
    public class UN_ArcIndicator : MonoBehaviour
    {
        // ---------------------------------------------------------------------------------------------------
        // Public interface. This sets what direction to use on throw and how fast it goes initially
        public void Set(float startSpeed, Quaternion rotation)
        // ---------------------------------------------------------------------------------------------------
        {
            _startSpeed = startSpeed;
            _rotation = rotation;
        }


        [SerializeField]
        GameObject _arcPiece = null;
        [SerializeField]
        float _arcDefaultLength = 1.0f;
        [SerializeField]
        LayerMask _layerMask = new LayerMask();       // used to mask what this arc hits
        [SerializeField]
        float _maxTravelDistance = 20.0f;
        [SerializeField]
        float _timePerSegmentInSeconds = 0.05f;


        List<GameObject> _arcPieceList = new List<GameObject>();
        Vector3 _arcPieceOriginalLocalScale;
        float _startSpeed;
        Quaternion _rotation;
        List<Vector3> _positions = new List<Vector3>();


        // ---------------------------------------------------------------------------------------------------
        void Awake()
        // ---------------------------------------------------------------------------------------------------
        {
            _arcPieceOriginalLocalScale = _arcPiece.transform.localScale;
            UN.SetActive(_arcPiece, false);
        }


        // -----------------------------------------------------------------------------------------------------------
        private void Update()
        // -----------------------------------------------------------------------------------------------------------
        {
            UpdateTrajectory(transform.position,                            // current position
                             _rotation * Vector3.forward * _startSpeed,     // current velocity
                             _timePerSegmentInSeconds,                      // time (i.e. distance) per segment 
                             _maxTravelDistance);                           // how far to trace arc
        }




        // ---------------------------------------------------------------------------------------------------
        GameObject MakeArcPiece()
        // ---------------------------------------------------------------------------------------------------
        {
            GameObject obj = GameObject.Instantiate(_arcPiece, transform) as GameObject;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.SetActive(false);
            return obj;
        }




        // -----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// This rebuilds our line segments to make an arc
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="velocity"></param>
        /// <param name="timePerSegmentInSeconds"></param>
        /// <param name="maxTravelDistance"></param>
        void UpdateTrajectory(Vector3 startPos, Vector3 velocity, float timePerSegmentInSeconds, float maxTravelDistance)
        // -----------------------------------------------------------------------------------------------------------
        {
            if (timePerSegmentInSeconds <= float.Epsilon)
                return;


            Vector3 currentPos = startPos;
            float traveledDistance = 0.0f;
            Vector3 newPosition;

            _positions.Clear();
            _positions.Add(startPos);
            while (traveledDistance < maxTravelDistance)    // don't make arc go on forever
            {
                bool hasHitSomething = TravelTrajectorySegment(currentPos, out newPosition, ref velocity, timePerSegmentInSeconds);
                _positions.Add(newPosition);
                if (hasHitSomething)
                {
                    break;
                }
                traveledDistance += Vector3.Distance(currentPos, newPosition);

                currentPos = newPosition;
            }

            BuildTrajectoryLine(_positions);
        }

        // -----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Computes the end point of the an arc based on strat point and current velocity. Current velocity is modified by gravity
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="velocity"></param>
        /// <param name="timePerSegmentInSeconds"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        bool TravelTrajectorySegment(Vector3 startPos, out Vector3 endPos, ref Vector3 velocity, float timePerSegmentInSeconds)
        // -----------------------------------------------------------------------------------------------------------
        {
            velocity += Physics.gravity * timePerSegmentInSeconds; // add gravity to our velocity
            endPos = startPos + velocity * timePerSegmentInSeconds;

            RaycastHit hitInfo;
            bool hasHitSomething = Physics.Linecast(startPos, endPos, out hitInfo, _layerMask);
            if (hasHitSomething)
            {
                endPos = hitInfo.point;
            }

            return hasHitSomething;
        }

        // -----------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  this moves our arc pieces to link the given positions together 
        /// </summary>
        /// <param name="positions"></param>
        void BuildTrajectoryLine(List<Vector3> positions)
        // -----------------------------------------------------------------------------------------------------------
        {
            Vector3 startPos;
            Vector3 endPos;
            Vector3 delta;

            Vector3 arcPieceScale = _arcPieceOriginalLocalScale;

            GameObject currentArcPiece = null;


            // make sure we have enough arc pieces
            for (int i = _arcPieceList.Count; i < positions.Count; i++)
            {
                _arcPieceList.Add(MakeArcPiece());
            }


            Debug.Assert(_arcPieceList.Count >= positions.Count);

            // start at 1, as we're always going to go from previous size to this one
            // also note that we use this after the below loop to set inactive pieces
            int currentPieceNdx = 1;

            for (; currentPieceNdx < positions.Count; ++currentPieceNdx)
            {
                currentArcPiece = _arcPieceList[currentPieceNdx - 1];
                UN.SetActive(currentArcPiece, true);

                // set positions
                startPos = positions[currentPieceNdx - 1];
                endPos = positions[currentPieceNdx];
                currentArcPiece.transform.position = startPos;

                // set scale

                delta = endPos - startPos;                              // gap between points
                arcPieceScale.z = delta.magnitude / _arcDefaultLength;  // stretch or shrink along z
                currentArcPiece.transform.localScale = arcPieceScale;// set scale


                // set rotations
                currentArcPiece.transform.rotation = Quaternion.LookRotation(delta.normalized);
            }

            // make sure pieces we don't use are disabled
            for (; currentPieceNdx < _arcPieceList.Count; currentPieceNdx++)
            {
                UN.SetActive(_arcPieceList[currentPieceNdx], false);
            }
        }
    }
}