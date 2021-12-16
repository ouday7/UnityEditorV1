using System.Collections.Generic;
using System.Linq;
using Envast.Components.GridLayout.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Envast.Components.GridLayout
{
    public class CustomGridLayout : BaseMonoBehaviour
    {
        public Alignment alignment;
        public Vector2 padding;
        public Vector2 spacing;
        public LayoutType layout;

        public bool includeDisabledChildren;
        //public bool forceNewLines;

        private RectTransform _rt;

        public RectTransform RectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }

        private float _currentLineWidth;
        private float _totalLinesHeight;
        private bool _isNewLine;
        private float _parentWidth;
        private List<float> _linesWidthList = new List<float>();
        
        [SerializeField] private string newLineTag = "LayoutNewLine";
        public string NewLineTag => newLineTag;

        
        
        
        //change here
        public override void ReleaseReferences()
        {
            _rt = null;
            _linesWidthList?.Clear();
            _linesWidthList = null;
        }

        private RectTransform GetFirstChild()
        {
            var child = includeDisabledChildren
                ? RectTransform.GetChild(0).GetRectTransform()
                : RectTransform.GetChildren().FirstOrDefault(ch => ch.gameObject.activeSelf);
            return child;
        }

        private void RestoreOriginalAnchor()
        {
            for (var i = 0; i < RectTransform.childCount; i++)
            {
                var rt = RectTransform.GetChild(i).GetRectTransform();
                var pos = rt.localPosition;     
                rt.SetAnchorsByType(FreeAnchorsTypes.MiddleCenter); 
                rt.localPosition = pos;
            }
        }
    
        [Button("Update Layout", ButtonSizes.Medium)]
        public void UpdateLayout()
        {
            if (RectTransform.childCount == 0) return;

            _currentLineWidth = 0f;
            _totalLinesHeight = 0f;
            _linesWidthList.Clear();
            _isNewLine = false;

            switch (alignment)
            {
                case Alignment.UpperLeft:
                    TopLeftGrid(layout);
                    break;
                case Alignment.UpperCenter:
                    TopCenterGrid(layout);
                    break;
                case Alignment.UpperRight:
                    TopRightGrid(layout);
                    break;
                case Alignment.MiddleLeft:
                    MiddleLeftVertical(layout);
                    break;
                case Alignment.MiddleCenter:
                    MiddleCenterVertical(layout);
                    break;
                case Alignment.MiddleRight:
                    MiddleRightVertical(layout);
                    break;
                case Alignment.LowerRight:
                    BottomRightVertical(layout);
                    break;
                case Alignment.LowerLeft:
                    BottomLeftVertical(layout);
                    break;
                case Alignment.LowerCenter:
                    BottomCenterVertical(layout);
                    break;
            }

            RestoreOriginalAnchor();
        }

        [Button("Shuffle", ButtonSizes.Medium)]
        public void ShuffleLayout()
        {
            if (RectTransform.childCount == 0) return;
            RectTransform.ShuffleSiblingIndexes();
            UpdateLayout();
        }

        private void BottomRightVertical(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.BottomRight);
                lastTreatedChild.SetAnchorX((lastTreatedChild.rect.width / 2) - _linesWidthList[0]);
                lastTreatedChild.SetAnchorY(-(lastTreatedChild.rect.height / 2) + padding.y + _totalLinesHeight);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineLTR(FreeAnchorsTypes.BottomRight, lastTreatedChild);
                else VariantMultiLineLTR(FreeAnchorsTypes.BottomRight, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.BottomRight);
                lastTreatedChild.SetAnchorX(-padding.x - (lastTreatedChild.rect.width / 2));
                lastTreatedChild.SetAnchorY(-(lastTreatedChild.rect.height / 2) + padding.y + _totalLinesHeight);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.BottomRight, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.BottomRight, lastTreatedChild);
            }
        }

        private void BottomLeftVertical(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.BottomLeft);
                lastTreatedChild.SetAnchorX(padding.x + (lastTreatedChild.rect.width / 2));
                lastTreatedChild.SetAnchorY(_totalLinesHeight - (lastTreatedChild.rect.height / 2) + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineLTR(FreeAnchorsTypes.BottomLeft, lastTreatedChild);
                else MultiLineLTR(FreeAnchorsTypes.BottomLeft, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.BottomLeft);
                lastTreatedChild.SetAnchorX(-(lastTreatedChild.rect.width / 2) + _linesWidthList[0]);
                lastTreatedChild.SetAnchorY(_totalLinesHeight - (lastTreatedChild.rect.height / 2) + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.BottomLeft, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.BottomLeft, lastTreatedChild);
            }
        }

        private void BottomCenterVertical(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.BottomLeft);
                lastTreatedChild.SetAnchorX(padding.x + (lastTreatedChild.rect.width / 2) +
                                            (_parentWidth - _linesWidthList[0]) / 2);
                lastTreatedChild.SetAnchorY(-(lastTreatedChild.rect.height / 2) + _totalLinesHeight + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineLTR(FreeAnchorsTypes.BottomLeft, lastTreatedChild);
                else VariantMultiLineLTR(FreeAnchorsTypes.BottomLeft, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.BottomLeft);
                lastTreatedChild.SetAnchorX(padding.x - (lastTreatedChild.rect.width / 2) +
                                            (_parentWidth - _linesWidthList[0]) / 2 + _linesWidthList[0]);
                lastTreatedChild.SetAnchorY(-(lastTreatedChild.rect.height / 2) + _totalLinesHeight + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.BottomLeft, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.BottomLeft, lastTreatedChild);
            }
        }

        private void MiddleRightVertical(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopRight);
                lastTreatedChild.SetAnchorX((lastTreatedChild.rect.width / 2) - _linesWidthList[0]);
                lastTreatedChild.SetAnchorY(-((RectTransform.rect.height - _totalLinesHeight) / 2) -
                    (lastTreatedChild.rect.height / 2) + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineLTR(FreeAnchorsTypes.TopRight, lastTreatedChild);
                else VariantMultiLineLTR(FreeAnchorsTypes.TopRight, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopRight);
                lastTreatedChild.SetAnchorX(-padding.x - (lastTreatedChild.rect.width / 2));
                lastTreatedChild.SetAnchorY(-((RectTransform.rect.height - _totalLinesHeight) / 2) -
                    (lastTreatedChild.rect.height / 2) + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.TopRight, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.TopRight, lastTreatedChild);
            }
        }

        private void MiddleCenterVertical(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopLeft);
                lastTreatedChild.SetAnchorX(padding.x + (lastTreatedChild.rect.width / 2) +
                                            (_parentWidth - _linesWidthList[0]) / 2);
                lastTreatedChild.SetAnchorY(-((RectTransform.rect.height - _totalLinesHeight) / 2) -
                    (lastTreatedChild.rect.height / 2) + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineLTR(FreeAnchorsTypes.TopLeft, lastTreatedChild);
                else VariantMultiLineLTR(FreeAnchorsTypes.TopLeft, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopRight);
                lastTreatedChild.SetAnchorX(-padding.x - (lastTreatedChild.rect.width / 2) -
                                            (_parentWidth - _linesWidthList[0]) / 2);
                lastTreatedChild.SetAnchorY(-((RectTransform.rect.height - _totalLinesHeight) / 2) -
                    (lastTreatedChild.rect.height / 2) + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.TopRight, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.TopRight, lastTreatedChild);
            }
        }

        private void MiddleLeftVertical(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopLeft);
                lastTreatedChild.SetAnchorX(padding.x + (lastTreatedChild.rect.width / 2));
                lastTreatedChild.SetAnchorY(-((RectTransform.rect.height - _totalLinesHeight) / 2) -
                    (lastTreatedChild.rect.height / 2) + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineLTR(FreeAnchorsTypes.TopLeft, lastTreatedChild);
                else MultiLineLTR(FreeAnchorsTypes.TopLeft, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopLeft);
                lastTreatedChild.SetAnchorX(-(lastTreatedChild.rect.width / 2) + _linesWidthList[0]);
                lastTreatedChild.SetAnchorY(-((RectTransform.rect.height - _totalLinesHeight) / 2) -
                    (lastTreatedChild.rect.height / 2) + padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.TopLeft, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.TopLeft, lastTreatedChild);
            }
        }

        private void TopRightGrid(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopRight);
                lastTreatedChild.SetAnchorX((lastTreatedChild.rect.width / 2) - _linesWidthList[0]);
                lastTreatedChild.SetAnchorY(-(lastTreatedChild.rect.height / 2) - padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineLTR(FreeAnchorsTypes.TopRight, lastTreatedChild);
                else VariantMultiLineLTR(FreeAnchorsTypes.TopRight, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopRight);
                lastTreatedChild.SetAnchorX(-padding.x - (lastTreatedChild.rect.width / 2));
                lastTreatedChild.SetAnchorY(-(lastTreatedChild.rect.height / 2) - padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.TopRight, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.TopRight, lastTreatedChild);
            }
        }

        private void TopCenterGrid(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopLeft);
                lastTreatedChild.SetAnchorX(padding.x + (lastTreatedChild.rect.width / 2) +
                                            (_parentWidth - _linesWidthList[0]) / 2);
                lastTreatedChild.SetAnchorY(-(lastTreatedChild.rect.height / 2) - padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1)
                    SingleLineLTR(FreeAnchorsTypes.TopLeft, lastTreatedChild);
                else
                    VariantMultiLineLTR(FreeAnchorsTypes.TopLeft, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopRight);
                lastTreatedChild.SetAnchorX(-padding.x - (lastTreatedChild.rect.width / 2) -
                                            (_parentWidth - _linesWidthList[0]) / 2);
                lastTreatedChild.SetAnchorY(-(lastTreatedChild.rect.height / 2) - padding.y);

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.TopRight, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.TopRight, lastTreatedChild);
            }
        }

        private void TopLeftGrid(LayoutType inLayout)
        {
            var lastTreatedChild = GetFirstChild();
            if (lastTreatedChild == null) return;

            _parentWidth = RectTransform.rect.width;

            UpdateWidthHeightData(lastTreatedChild);

            if (inLayout == LayoutType.LTR)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopLeft);
                lastTreatedChild.SetAnchorX(padding.x + (lastTreatedChild.rect.width / 2));
                lastTreatedChild.SetAnchorY(-padding.y - (lastTreatedChild.rect.height / 2));

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1)
                    SingleLineLTR(FreeAnchorsTypes.TopLeft, lastTreatedChild);
                else
                    MultiLineLTR(FreeAnchorsTypes.TopLeft, lastTreatedChild);
            }

            if (inLayout == LayoutType.RTL)
            {
                lastTreatedChild.SetAnchorsByType(FreeAnchorsTypes.TopLeft);
                lastTreatedChild.SetAnchorX(-(lastTreatedChild.rect.width / 2) + _linesWidthList[0]);
                lastTreatedChild.SetAnchorY(-padding.y - (lastTreatedChild.rect.height / 2));

                _currentLineWidth = padding.x + lastTreatedChild.rect.width;

                if (_linesWidthList.Count == 1) SingleLineRTL(FreeAnchorsTypes.TopLeft, lastTreatedChild);
                else MultiLineRTL(FreeAnchorsTypes.TopLeft, lastTreatedChild);
            }
        }

        private void MultiLineLTR(FreeAnchorsTypes anchor, RectTransform last)
        {
            var lastChild = last;

            for (var i = 1; i < RectTransform.childCount; i++)
            {
                var child = RectTransform.GetChild(i).GetRectTransform();
                if (!includeDisabledChildren && !child.gameObject.activeSelf) continue;

                if (lastChild == child) continue;

                /*if (forceNewLines)
            {
                if (child.TryGetComponent(out NewLineGrid nl))
                {
                    //child.sizeDelta = Vector2.one;
                    child.sizeDelta = Vector2.zero;
                    _isNewLine = true;
                    continue;
                }
            }*/

                Vector2 newPos;
                child.SetAnchorsByType(anchor);
            
                if ((_currentLineWidth + (spacing.x + child.rect.width)) > _parentWidth || _isNewLine)
                {
                    _isNewLine = false;
                    _currentLineWidth = padding.x + child.rect.width;
                    newPos = CalculatePosLTR(true, last, child);
                }
                else
                {
                    _currentLineWidth = _currentLineWidth + (child.rect.width + spacing.x);
                    newPos = CalculatePosLTR(false, last, child);
                }

                child.anchoredPosition = newPos;
                last = child;
            }
        }

        private void MultiLineRTL(FreeAnchorsTypes anchor, RectTransform last)
        {
            var lineCounter = 1;
            var lastChild = last;
        
            while (lineCounter < _linesWidthList.Count)
            {
                for (var i = 1; i < RectTransform.childCount; i++)
                {
                    var child = RectTransform.GetChild(i).GetRectTransform();
                    if (!includeDisabledChildren && !child.gameObject.activeSelf) continue;

                    if (lastChild == child) continue;

                    /*if (forceNewLines)
                    {
                        if (child.TryGetComponent(out NewLineGrid nl))
                        {
                            //child.sizeDelta = Vector2.one;
                            child.sizeDelta = Vector2.zero;
                            _isNewLine = true;
                            continue;
                        }
                    }*/
                    
                    Vector2 newPos;
                    child.SetAnchorsByType(anchor);
                
                    if (_currentLineWidth + (spacing.x + child.rect.width) > _parentWidth || _isNewLine)
                    {
                        _isNewLine = false;
                        _currentLineWidth = padding.x + child.rect.width;
                        newPos = CalculatePosRTL(true, last, child, lineCounter);
                    
                        lineCounter++;
                    }
                    else
                    {
                        _currentLineWidth = _currentLineWidth + (child.rect.width + spacing.x);
                        newPos = CalculatePosRTL(false, last, child);
                    }

                    child.anchoredPosition = newPos;
                    last = child;
                }
            }
        }

        private void SingleLineLTR(FreeAnchorsTypes anchor, RectTransform last)
        {
            var lastChild = last;
            for (var i = 1; i < RectTransform.childCount; i++)
            {
                var child = RectTransform.GetChild(i).GetRectTransform();
                if (!includeDisabledChildren && !child.gameObject.activeSelf) continue;

                if (lastChild == child) continue;

                var xPos = 0f;
                var yPos = 0f;

                child.SetAnchorsByType(anchor);
                if (_currentLineWidth + (spacing.x + child.rect.width) < _parentWidth)
                {
                    _currentLineWidth = _currentLineWidth + (child.rect.width + spacing.x);
                    var anchoredPosition = last.anchoredPosition;
                    yPos = anchoredPosition.y;
                    xPos = anchoredPosition.x + (last.sizeDelta.x / 2 + spacing.x + child.sizeDelta.x / 2);
                }

                child.anchoredPosition = new Vector2(xPos, yPos);
                last = child;
            }
        }

        private void SingleLineRTL(FreeAnchorsTypes anchor, RectTransform last)
        {
            var lastChild = last;
            for (var i = 1; i < RectTransform.childCount; i++)
            {
                var child = RectTransform.GetChild(i).GetRectTransform();

                if (!includeDisabledChildren && !child.gameObject.activeSelf) continue;

                if (lastChild == child) continue;

                var xPos = 0f;
                var yPos = 0f;

                child.SetAnchorsByType(anchor);

                if (_currentLineWidth + (spacing.x + child.rect.width) < _parentWidth)
                {
                    _currentLineWidth = _currentLineWidth + (child.rect.width + spacing.x);
                    var anchoredPosition = last.anchoredPosition;
                    yPos = anchoredPosition.y;
                    xPos = anchoredPosition.x - (last.sizeDelta.x / 2 + spacing.x + child.sizeDelta.x / 2);
                }

                child.anchoredPosition = new Vector2(xPos, yPos);

                last = child;
            }
        }

        private void UpdateWidthHeightData(RectTransform lastTreatedChild)
        {
            var isFirstItem = true;
            var lastActiveChildIndex = 0;

            var lastRect = lastTreatedChild.rect;
            _currentLineWidth = padding.x + lastRect.width;
            _totalLinesHeight = lastRect.height;

            if (RectTransform.childCount > 1)
            {
                isFirstItem = false;
                for (var i = 1; i < RectTransform.childCount; i++)
                {
                    if (RectTransform.GetChild(i).gameObject.activeSelf) lastActiveChildIndex = i;
                }

                if (lastActiveChildIndex == 0)
                {
                    _linesWidthList.Add(_currentLineWidth);
                }
            }
            else
            {
                _linesWidthList.Add(_currentLineWidth);
            }

            for (var i = 1; i < RectTransform.childCount; i++)
            {
                var child = RectTransform.GetChild(i).GetRectTransform();
                if (!includeDisabledChildren && !child.gameObject.activeSelf) continue;

                /*if (forceNewLines)
                {
                    if (child.TryGetComponent(out NewLineGrid nl))
                    {
                        _isNewLine = true;
                        continue;
                    }
                }*/
                
                if (child.CompareTag(newLineTag))
                {
                    // change made here: [- 2 * spacing instead of one => to place new line at the end forcing the creation of new line not starting a new line]
                    child.sizeDelta = new Vector2(_parentWidth - (_currentLineWidth + spacing.x * 2),
                        RectTransform.GetChild(i - 1).GetRectTransform().sizeDelta.y);
                    //wrong size for the new Line go
                    // Debug.Log($"//. Current Line width = {_currentLineWidth} Create new line with width {_parentWidth - (_currentLineWidth + spacing.x)}");
                }
                
                if (_currentLineWidth + (spacing.x + child.rect.width) > _parentWidth)
                {
                    _linesWidthList.Add(_currentLineWidth);
                    var rect = child.rect;
                    _currentLineWidth = padding.x + rect.width;
                    _totalLinesHeight += rect.height + spacing.y;
                }
                else
                {
                    if (child != lastTreatedChild) _currentLineWidth += child.rect.width + spacing.x;
                }

                if (child == RectTransform.GetChild(lastActiveChildIndex) && !isFirstItem)
                {
                    _linesWidthList.Add(_currentLineWidth);
                }
            }

            /*foreach (var line in _linesWidthList)
        {
            print("line: " + line);
        }*/
        }

        private Vector2 CalculatePosLTR(bool newLine, RectTransform last, RectTransform child, int lineIndex = 0)
        {
            float xPos = 0;
            float yPos = 0;
            var childRect = child.rect;

            if (!newLine)
            {
                yPos = last.anchoredPosition.y;
                xPos = last.anchoredPosition.x + (last.sizeDelta.x / 2 + spacing.x + child.sizeDelta.x / 2);
                return new Vector2(xPos, yPos);
            }

            switch (alignment)
            {
                case Alignment.UpperLeft:
                case Alignment.MiddleLeft:
                case Alignment.LowerLeft:
                    yPos = last.anchoredPosition.y - (childRect.height + spacing.y);
                    xPos = padding.x + childRect.width / 2;
                    break;
                case Alignment.UpperCenter:
                case Alignment.MiddleCenter:
                case Alignment.LowerCenter:
                    yPos = last.anchoredPosition.y - (childRect.height + spacing.y);
                    xPos = padding.x + childRect.width / 2 + (_parentWidth - _linesWidthList[lineIndex]) / 2;
                    break;
                case Alignment.UpperRight:
                case Alignment.MiddleRight:
                case Alignment.LowerRight:
                    yPos = last.anchoredPosition.y - (childRect.height + spacing.y);
                    xPos =  childRect.width / 2 - _linesWidthList[lineIndex];
                    break;
            }

            return new Vector2(xPos, yPos);
        }
    
        private Vector2 CalculatePosRTL(bool newLine, RectTransform last, RectTransform child, int lineIndex = 0)
        {
            float xPos = 0;
            float yPos = 0;
            var childRect = child.rect;
            var lastAnchoredPos = last.anchoredPosition;

            if (!newLine)
            {
                yPos = lastAnchoredPos.y;
                xPos = lastAnchoredPos.x - (last.sizeDelta.x / 2 + spacing.x + child.sizeDelta.x / 2);
                return new Vector2(xPos, yPos);
            }

            switch (alignment)
            {
                case Alignment.UpperLeft:
                case Alignment.MiddleLeft:
                case Alignment.LowerLeft:
                    yPos = lastAnchoredPos.y - (childRect.height + spacing.y);
                    xPos = _linesWidthList[lineIndex] - childRect.width / 2;
                    break;
                case Alignment.UpperCenter:
                case Alignment.MiddleCenter:
                    yPos = lastAnchoredPos.y - (childRect.height + spacing.y);
                    xPos = padding.x - childRect.width / 2 - (_parentWidth - _linesWidthList[lineIndex]) / 2;
                    break;
                case Alignment.UpperRight:
                case Alignment.MiddleRight:
                case Alignment.LowerRight:
                    yPos = lastAnchoredPos.y - (childRect.height + spacing.y);
                    xPos = -padding.x - childRect.width / 2;
                    break;
                case Alignment.LowerCenter:
                    yPos = lastAnchoredPos.y - (childRect.height + spacing.y);
                    xPos = padding.x - childRect.width / 2 + (_parentWidth - _linesWidthList[lineIndex]) / 2 + _linesWidthList[lineIndex];
                    break;
            }

            return new Vector2(xPos, yPos);
        }

        private void VariantMultiLineLTR(FreeAnchorsTypes anchor, RectTransform last)
        {
            var lineCounter = 1;
            var lastChild = last;
            while (lineCounter < _linesWidthList.Count)
            {
                for (var i = 1; i < RectTransform.childCount; i++)
                {
                    var child = RectTransform.GetChild(i).GetRectTransform();
                    if (!includeDisabledChildren && !child.gameObject.activeSelf) continue;

                    if (lastChild == child) continue;

                    /*if (forceNewLines)
                    {
                        if (child.TryGetComponent(out NewLineGrid nl))
                        {
                            //child.sizeDelta = Vector2.one;
                            child.sizeDelta = Vector2.zero;
                            _isNewLine = true;
                            continue;
                        }
                    }*/

                    Vector2 newPos;
                    child.SetAnchorsByType(anchor);
                
                    if (_currentLineWidth + (spacing.x + child.rect.width) > _parentWidth || _isNewLine)
                    {
                        _isNewLine = false;
                        _currentLineWidth = padding.x + child.rect.width;
                        newPos = CalculatePosLTR(true, last, child, lineCounter);

                        lineCounter++;
                    }
                    else
                    {
                        _currentLineWidth = _currentLineWidth + (child.rect.width + spacing.x);
                        newPos = CalculatePosLTR(false, last, child);
                    }

                    child.anchoredPosition = newPos;
                    last = child;
                
                }
            }
        }
        
        public float CalculateScrollHeight(int nbElementsOrRows, float elemHeight) => nbElementsOrRows * elemHeight + spacing.y * (nbElementsOrRows - 1) + padding.y * 2;

    }
}

//}
