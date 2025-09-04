using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Zombie zombie; // Vari�vel para refer�ncia ao script principal do zumbi

    public float chaseSpeed = 6f;
    public float attackingDistance = 2.5f;

    private FieldOfView fov;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        fov = animator.GetComponent<FieldOfView>();
        zombie = animator.GetComponent<Zombie>(); // Pega o componente Zombie no in�cio

        agent.speed = chaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);

        Vector3 direction = player.position - animator.transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // L�gica de transi��o corrigida
        if (fov.canSeePlayer)
        {
            // Se pode ver o jogador, atualiza a �ltima posi��o conhecida
            zombie.lastKnownPosition = player.position;
        }
        else
        {
            // Se perdeu o jogador de vista, para de perseguir e come�a a procurar
            animator.SetBool("isChasing", false);
            animator.SetBool("isSearching", true);
        }

        // L�gica de ataque permanece a mesma
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}