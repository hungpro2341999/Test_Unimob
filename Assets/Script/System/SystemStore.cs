using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SystemStore : SingletonX<SystemStore>
{
    public int MAX_CLIENT = 1;
    public List<Client> All_Client
    {
        get
        {
            return allClient;
        }
    }
    public List<Employer> All_Employer
    {
        get
        {
            return allEmployer;
        }
    }
    public List<Tree> All_Tree
    {
        get
        {
            return listTree;
        }
    }
    public Tree GetTreeByID(int ID)
    {
        foreach(var tree in listTree)
        {
            if(tree.Index == ID)
            {
                return tree;
            }
        }
        return null;
    }
    [SerializeField]
    List<Person> allPersons = new List<Person>();
    [SerializeField]
    List<Tree> listTree;
    List<AvailableSlot> allPosStandClient;
    [SerializeField]
    List<Client> allClient = new List<Client>();
    [SerializeField]
    List<Employer> allEmployer = new List<Employer>();
    [SerializeField]
    List<Person> listClientRemove = new List<Person>();
    System.Action actionUpdate = null;
    Transform posStartClient;
    Transform posStartEmployer;

    void Start()
    {
        posStartEmployer = transform.FindChildByName("posStartEmployer");
        posStartClient = transform.FindChildByName("posStart");
        listTree = new List<Tree>(FindObjectsOfType<Tree>());
        allPersons.Clear();
        foreach (var person in FindObjectsOfType<Employer>())
        {
            person.Init();
            AddPerson(person);
        }

        var rootPosStand = transform.FindChildByName("All_Pos_Stand");
        allPosStandClient = new List<AvailableSlot>();
        foreach (Transform child in rootPosStand)
        {
            var availableSlot = child.gameObject.AddComponent<AvailableSlot>();
            allPosStandClient.Add(availableSlot);
        }
        DoUpdateSpawnClient();

        EventBus.Subscribe<EventOpenTree>((evt) =>
        {
            var tree = evt.tree;
            AddTree(tree);
            SpawnEmployer();
        });
    }
    
    void Update()
    {
        foreach (var person in allPersons)
        {
            person.DoUpdate();
            if (person.isRemove)
            {
                RemovePerson(person);
            }
        }
        foreach (var tree in listTree)
        {
            tree.DoUpdate();
        }
        actionUpdate?.Invoke();
    }

    void DoUpdateSpawnClient()
    {
        float t = 1;
        actionUpdate = () =>
       {
           t += Time.deltaTime;
           if (t > 1)
           {
               if (allClient.Count < MAX_CLIENT)
               {
                   t = 0;
                   SpawnClient();
               }
           }
       };
    }
    void SpawnClient()
    {
        var client = MyFactory.InstantiatePreb("Client").GetComponent<Client>();
        client.Status = Person.TypeStatus.Waiting;
        client.Init();
        var availableSlot = GetPosAvailable();
        client.AddActionClientGetItemDone(() =>
        {
            availableSlot.IsAvailable = true;
        });
        client.DoMove(availableSlot.transform.position, () =>
        {
            client.Status = Person.TypeStatus.Ready;
        });
        client.transform.position = posStartClient.position;
        AddPerson(client);
    }
    public void SpawnEmployer()
    {
        var employer = MyFactory.InstantiatePreb("Employer").GetComponent<Employer>();
        employer.Init();
        employer.transform.position = posStartEmployer.position;
        //SystemActionStore.Instance.AddToWait(employer);
        AddPerson(employer);
    }
    AvailableSlot GetPosAvailable()
    {
        List<AvailableSlot> listAvailable = new List<AvailableSlot>();
        foreach (var pos in allPosStandClient)
        {
            if (pos.IsAvailable)
            {
                pos.IsAvailable = false;
                return pos;
            }
        }
        return null;
    }
    void AddPerson(Person person)
    {
        allPersons.Add(person);
        if (person is Client)
        {
            allClient.Add((Client)person);
        }
        else if (person is Employer)
        {
            allEmployer.Add((Employer)person);
        }
    }
    void RemovePerson(Person person)
    {
        listClientRemove.Add(person);
        if (person is Client)
        {
            allClient.Remove((Client)person);
        }
        else if (person is Employer)
        {
            allEmployer.Remove((Employer)person);
        }
    }
    void AddTree(Tree tree)
    {        
        listTree.Add(tree);
    }
   

}
public class AvailableSlot : MonoBehaviour
{
    public bool IsAvailable = true;
}
