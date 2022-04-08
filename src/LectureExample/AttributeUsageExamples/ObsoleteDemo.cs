namespace LectureExample.AttributeUsageExamples;

public static class ObsoleteDemo
{
    [Obsolete("Please use DoSomethingV2 instead!")]
    public static void DoSomethingV1()
    {
    }

    public static int DoSomethingV2()
    {
        return 42;
    }
}

public class UsageExample
{
    public static void Example()
    {
        var usageOk = ObsoleteDemo.DoSomethingV2();
        Console.WriteLine("Result is {0}", usageOk);

        // warning
        ObsoleteDemo.DoSomethingV1();
    }
}
