using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit.Utilities
{
    public class UN_VisualLine : MonoBehaviour
    {
        [SerializeField]
        GameObject _piece = null;

        [SerializeField]
        GameObject _end = null;


        [SerializeField]
        float _pieceDefaultLength = 1.0f;   // so when we scale it, we know how much to scale by

        Vector3 _arcPieceOriginalLocalScale;
        List<GameObject> _arcPieceList = new List<GameObject>();
        List<Vector3> _positions = new List<Vector3>();

        // ---------------------------------------------------------------------------------------------------
        void Awake()
        // ---------------------------------------------------------------------------------------------------
        {
            _arcPieceOriginalLocalScale = _piece.transform.localScale;
            UN.SetActive(_piece, false);
        }



        // ---------------------------------------------------------------------------------------------------
        GameObject MakeArcPiece()
        // ---------------------------------------------------------------------------------------------------
        {
            GameObject obj = GameObject.Instantiate(_piece, transform) as GameObject;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.SetActive(false);
            return obj;
        }



        // -----------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  this moves our arc pieces to link the given positions together 
        /// </summary>
        /// <param name="positions"></param>
        public void SetLine(List<Vector3> positions)
        // -----------------------------------------------------------------------------------------------------------
        {
            // first quick and dirty - if we don't have any positions, hide everything
            if (positions.Count < 2)
            {
                for (int i = 0;  i < _arcPieceList.Count; i++)
                    UN.SetActive(_arcPieceList[i], false);
                return;
            }
           
            int numSegRequired = positions.Count - 1;

            // make sure we have enough arc pieces
            for (int i = _arcPieceList.Count; i < numSegRequired; i++)
            {
                _arcPieceList.Add(MakeArcPiece());
            }


            Debug.Assert(_arcPieceList.Count >= positions.Count-1);



            // start at 1, as we're always going to go from previous size to this one
            // also note that we use this after the below loop to set inactive pieces
            int curSegment = 0;
            Vector3 startPos;
            Vector3 endPos;
            Vector3 delta;
            GameObject currentArcPiece = null;
            Vector3 arcPieceScale = _arcPieceOriginalLocalScale;    // we are going to scale z to stretch the line

            for (; curSegment < positions.Count-1; ++curSegment)
            {
                currentArcPiece = _arcPieceList[curSegment];
                UN.SetActive(currentArcPiece, true);

                // set positions
                startPos = positions[curSegment];
                endPos = positions[curSegment+1];
                currentArcPiece.transform.position = startPos;

                // set scale

                delta = endPos - startPos;                              // gap between points
                arcPieceScale.z = delta.magnitude / _pieceDefaultLength;  // stretch or shrink along z
                currentArcPiece.transform.localScale = arcPieceScale;// set scale


                // set rotations
                currentArcPiece.transform.rotation = Quaternion.LookRotation(delta.normalized);
            }

            // make sure pieces we don't use are disabled
            for (; curSegment < _arcPieceList.Count; curSegment++)
            {
                UN.SetActive(_arcPieceList[curSegment], false);
            }

            if (_end != null)
            {
                if (positions.Count > 0)
                    _end.transform.position = positions[positions.Count - 1];
                else UN.SetActive(_end, false);
            }
        }
    }
}
