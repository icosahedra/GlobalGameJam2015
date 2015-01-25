using UnityEngine;
using System.Collections;


namespace Icosahedra{
public class AnimationNode : ScriptableObject {

	public AnimationNode(AnimationClip clip){
		this.clip = clip;	
	}

	[SerializeField] private AnimationClip clip;
	
	[SerializeField] private int layer;

	[SerializeField] private AnimationBlendMode blendMode;
	
	[SerializeField] private float speed = 1;
	
	[SerializeField] private WrapMode wrapMode = WrapMode.Loop;
	

	public AnimationClip Clip{
		get{
			return clip;	
		}	
		set{
			clip = value;	
		}
	}
	public int Layer{
		get{
			return layer;	
		}	
		set{
			layer = value;	
		}
	}

	public AnimationBlendMode BlendMode{
		get{
			return blendMode;	
		}
		set{
			blendMode = value;	
		}
	}
	
	public float Speed{
		get{
			return speed;	
		}	
		set{
			speed = value;	
		}
	}

	public WrapMode WrapMode{
		get{
			return wrapMode;	
		}	
		set{
			wrapMode = value;	
		}
	}
}

}
