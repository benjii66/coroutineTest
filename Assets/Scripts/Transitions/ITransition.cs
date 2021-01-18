using System.Collections;

public interface ITransition
{
    IEnumerable Enter();
    IEnumerable Exit();
}