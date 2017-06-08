using Assets.Scripts.Terrains;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.TerrainHelpers
{
    // ReSharper disable once InconsistentNaming
    public class TerraWarInfoUI : MonoBehaviour
    {
        public enum PositionE
        {
            Top = 0,
            Right = 1,
            Bottom = 2,
            Left = 3
        }
        public Image Back;
        public Text Level;
        public Image Force;

        public PositionE Position;

        public float FadeSpeed = 1f;
        private bool _isShow;

        public void Awake()
        {
            Back.color = new Color(1f, 1f, 1f, 0f);
            Level.color = new Color(1f, 1f, 1f, 0f);
            Force.color = new Color(1f, 1f, 1f, 0f);
            _isShow = false;
            Hide(true);
        }

        public void Update()
        {
            if (!_isShow)
                return;
            var p = transform.parent.InverseTransformPoint(GlobalMapGenerator2.I.Player.transform.position);
            var p0 = transform.localPosition;
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if ((Position == PositionE.Top || Position == PositionE.Bottom) && p.x >= -13.8f && p.x <= 13.8f)
            {
                p0.x = p.x;
            }
            else if ((Position == PositionE.Left || Position == PositionE.Right) && p.z >= -13.8f && p.z <= 13.8f)
            {
                p0.z = p.z;
            }

            transform.localPosition = p0;
        }

        public void Show()
        {
            if (_isShow)
                return;
            _isShow = true;
            Update();

            Back.enabled = true;
            Force.enabled = true;
            Level.enabled = true;

            Back.DOFade(1f, FadeSpeed);
            Force.DOFade(1f, FadeSpeed);
            Level.DOFade(1f, FadeSpeed);
        }

        private bool _isHideNow;
        public void Hide(bool force = false)
        {
            if (!force)
            {
                if (!_isShow || _isHideNow)
                    return;
            }

            _isHideNow = true;
            Back.DOFade(0f, FadeSpeed);
            Force.DOFade(0f, FadeSpeed);
            Level.DOFade(0f, FadeSpeed).OnComplete(() =>
            {
                Back.enabled = false;
                Force.enabled = false;
                Level.enabled = false;
                _isHideNow = false;
                _isShow = false;
            });
        }

        public void UpdateInfo(int levelValue, float forceValue, float maxForceValue)
        {
            Level.text = levelValue.ToString();
            if (forceValue == 0)
            {
                Force.fillAmount = 0;
            }
            else
            {
                Force.fillAmount = maxForceValue / forceValue;
            }
        }
    }
}
