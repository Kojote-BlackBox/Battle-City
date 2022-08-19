using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNode
{
    public AnimationNode nextNode;
    public AnimationNode previousNode;
    public AnimationNode currentAnimation;
    private string animationName;
    private Vector2 animationDirection;
    private bool animationCircle = false;

    public AnimationNode() {
        nextNode = this;
        previousNode = this;
    }

    public Vector2 GetCurrentAnimationDirection() {
        return currentAnimation.animationDirection;
    }

    public AnimationNode(string name, float direction) {
        nextNode = this;
        previousNode = this;
        animationName = name;
        animationDirection = DegreeToDirection(direction);

        if(currentAnimation == null) {
            currentAnimation = new AnimationNode();
            currentAnimation.nextNode = currentAnimation;
            currentAnimation.previousNode = currentAnimation;
        }
    }

    public void SetNextAnimation() {
        currentAnimation.nextNode = currentAnimation.nextNode.nextNode;
        currentAnimation.animationName = currentAnimation.nextNode.animationName;
        currentAnimation.animationDirection = currentAnimation.nextNode.animationDirection;
    }

    public void SetPreviousAnimation() {
        currentAnimation.nextNode = currentAnimation.nextNode.previousNode;
        currentAnimation.animationName = currentAnimation.nextNode.animationName;
        currentAnimation.animationDirection = currentAnimation.nextNode.animationDirection;
    }

    public string Play(string playAnimationName) {
        if(currentAnimation.nextNode == currentAnimation) {
            currentAnimation.nextNode = this;
            currentAnimation.animationName = this.animationName;
        }
       
        return SearchNextAnimationNode(playAnimationName);
    }

    private string SearchNextAnimationNode(string targetAnimation) {

        int nextSteps = 0;
        int previosSteps = 0;

        AnimationNode currentAnimationNode = this.currentAnimation.nextNode;

        while(currentAnimationNode.animationName != targetAnimation) {
            
            currentAnimationNode = currentAnimationNode.nextNode;
            nextSteps++;

            // primitive endless loop protection
            if (previosSteps > 1000) {
                Debug.Log("SearchNextAnimationNode() on AnimaionNode Class: " + targetAnimation + "Not Exist and occure a endless while");
                break;
            }
        }

        currentAnimationNode = this.currentAnimation.nextNode;

        while (currentAnimationNode.animationName != targetAnimation) {
            previosSteps++;
            currentAnimationNode = currentAnimationNode.previousNode;

            // primitive endless loop protection
            if (previosSteps > 1000) {
                Debug.Log("SearchNextAnimationNode() on AnimaionNode Class: " + targetAnimation + "Not Exist and occure a endless while");
                break;
            }
        }

        if (currentAnimation.animationName == targetAnimation) {
            return currentAnimation.animationName;

        } else if(nextSteps > previosSteps) {
            currentAnimation.nextNode = currentAnimation.nextNode.previousNode;
        } else {
            currentAnimation.nextNode = currentAnimation.nextNode.nextNode;
        }

        currentAnimation.animationName = currentAnimation.nextNode.animationName;
        currentAnimation.animationDirection = currentAnimation.nextNode.animationDirection;

        return currentAnimation.animationName;
    }

    public void AddNode(string name, float directionDegree) {

        AnimationNode newNode = new AnimationNode(name, directionDegree);

        if (this.nextNode == this) {
            this.nextNode = newNode;
            newNode.previousNode = this;
        } else {
            AnimationNode lastNote = GetLastNode();
            lastNote.nextNode = newNode;
            newNode.previousNode = lastNote;
        }
    }

    private AnimationNode GetLastNode() {
        if (animationCircle != true) {

            AnimationNode lastNode = this;
            while (lastNode != lastNode.nextNode)
            {
                lastNode = lastNode.nextNode;
            }
            return lastNode;

        } else {
            Debug.Log("Animation Circle Closed - GetLastNode return null");
            return null;
        }
    }

    public void CloseToLoop() {
        AnimationNode head = this;
        AnimationNode tail = this;

        while (tail != tail.nextNode)
        {
            tail.animationCircle = true;
            tail = tail.nextNode;

            // Delete Current State of all nodes exept head. Make it Unique
            if(tail != head) {
                tail.currentAnimation = null;
            }
        }

        tail.nextNode = head;
        head.previousNode = tail;
    }

    public float DirectionToDegree(Vector2 direction)
    {
        float degree = Vector2.Angle(Vector2.up, direction);
        Vector3 cross = Vector3.Cross(Vector2.up, direction);


        if (cross.z > 0)
        {
            degree = 360 - degree;
        }

        return degree;
    }

    public Vector2 DegreeToDirection(float degree)
    {
        float radians = -degree * Mathf.Deg2Rad;
        radians += 1.5708f;
        Vector2 returnDirection = new Vector2((float)Mathf.Cos(radians), (float)Mathf.Sin(radians));

        //return (Vector2)(Quaternion.Euler(0, 0, -degree) * Vector2.up);
        return returnDirection;
    }

    public void DebugLog() {
        if (animationCircle != true) {
            AnimationNode lastNode = this;
            while (lastNode != lastNode.nextNode)
            {
                Debug.Log(lastNode.animationName);
                lastNode = lastNode.nextNode;
            }
            Debug.Log(lastNode.animationName);
        } else {
            Debug.Log("Animation Circle Closed");
        }
    }
}
