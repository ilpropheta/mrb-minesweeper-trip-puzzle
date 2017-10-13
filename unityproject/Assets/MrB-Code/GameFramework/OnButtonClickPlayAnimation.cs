using System.Collections;
using System.Collections.Generic;
using Prime31.ZestKit;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnButtonClickPlayAnimation : MonoBehaviour
{
    public Animator m_animator;
    public string m_animationName;

	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnClick()
    {
        m_animator.Play(Animator.StringToHash(m_animationName));
    }
}
