using Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Capsule
{
    public class GridPossibilityLanding : MonoBehaviour
    {
        public static GridPossibilityLanding I { get; private set; }

        public Transform Quad;
        public Material MPossible;
        public Material MInPossible;

    
        //шаг сетки
        private const float Increment = 0.5f;

        private Transform[,] _quads;
        private AstarPath Astar { get; set; }
        private readonly float _heightAbove = 0.1f;
        private bool _isMove;

        public void Awake()
        {
            I = this;
        }

        public void Init ()
        {
            if (AstarPath.active != null)
                Astar = AstarPath.active;
            else
            {
                Debug.LogError("Not Found Astar");
                return;
            }
            GenerateGrid(6);
        }

        public bool CheckPossibilityLanding()
        {   
            //кол-во непроходимых ячеек
            var countInWalkable = 0;
       
            var count = _quads.GetLength(0);
            for (var i = 0; i < count ; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    var q = _quads[i,j];
                    var nnConstraint = new NNConstraint
                    {
                        constrainDistance = false,
                        constrainWalkability = false,
                        constrainArea = true
                    };

                    var gridNode = AstarPath.active.GetNearest(q.position, nnConstraint);
                    if (gridNode.node != null && gridNode.node.Walkable && Vector3.Distance(q.position, (Vector3)gridNode.node.position) < 0.5f)
                    {
                        q.GetComponentInChildren<Renderer>().material = MPossible;
                    }
                    else
                    {
                        countInWalkable++;
                        q.GetComponentInChildren<Renderer>().material = MInPossible;
                    }
                }
            }

            if (countInWalkable > 0)
            {
                return false;
            }

            return true;
        }

        public void GenerateGrid(int sizeOfCheck)
        {
            var length = (sizeOfCheck*2) + 1;

            if (_quads != null)
                ClearGrid();
            //if (quads == null || quads.Length > 0)
            _quads = new Transform[length, length];
        
            var posN = transform.position - new Vector3(length/2f * Increment, 0, length/2f * Increment);
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < length; j++)
                {
                    var pos = posN + new Vector3(Increment * j, 0,  Increment * i);
                    var g = Instantiate(Quad, pos, Quaternion.identity);
                    _quads[i,j] = g;
                    g.parent = transform;
                }
            }
        }

    
        public bool MoveGrid(Vector3 point)
        {
            var pos= new Vector3();
            var preuPos = transform.position;
            var nNode = Astar.GetNearest(point, NNConstraint.None);

            if(nNode.node != null)
                pos = (Vector3)nNode.node.position + new Vector3(0, _heightAbove, 0);
        
            if (pos != preuPos)
            {
                _isMove = true;
                transform.position = pos;   
            }

            return _isMove;
        }


        private void ClearGrid()
        {
            foreach (var q in _quads)
            {
                Destroy(q.gameObject);
            }
            _quads = null;
        }
    }
}
