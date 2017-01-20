using UnityEngine;
using System.Collections;

public interface IDamageable {



	 int Hp 
		{
		get;
		set;
		}

	void TakeDamage(int n);
	void Die();
	

}
