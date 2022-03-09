/*
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * 
 * Глава_7_2: Состояние
 * 
 * - позволяет объекту поменять свое поведение в зависимости от состояния
 */

//объектное представление логического состояния
interface IState
{
    void SetStateTrue(Variable v);
    void SetStateFalse(Variable v);
}

class Variable
{
    IState value;
    public Variable()
    {
        value = new StateFalse();
    }
    public void SetState(IState state) => value = state;
    public void False() => value.SetStateFalse(this);
    public void True() => value.SetStateTrue(this);
    public override string ToString() => value.ToString();
}

class StateFalse : IState
{
    public void SetStateFalse(Variable v) { Console.WriteLine("Итак в лжи"); }

    public void SetStateTrue(Variable v) => v.SetState(new StateTrue());
    public override string ToString() => "False";
}

class StateTrue : IState
{
    public void SetStateFalse(Variable v) => v.SetState(new StateFalse());

    public void SetStateTrue(Variable v) { Console.WriteLine("Итак в истине"); }
    public override string ToString() => "True";
}

//пример из реалной жизни
class Camera : ICameraState
{
    ICameraState state;
    public Camera() => state = new OffState();
    public void SetState(ICameraState state) => this.state = state;
    public void RecordVideo(Camera camera) => state.RecordVideo(camera);
    public void TakePictures(Camera camera) => state.TakePictures(camera);
    public void TurnOff(Camera camera) => state.TurnOff(camera);
    public void TurnOn(Camera camera) => state.TurnOn(camera);
}

interface ICameraState
{
    void TurnOn(Camera camera);
    void TurnOff(Camera camera);
    void TakePictures(Camera camera);
    void RecordVideo(Camera camera);
}

class FotoState : ICameraState
{
    public void RecordVideo(Camera camera) 
    {
        Console.WriteLine("Переходим в режим видео");
        camera.RecordVideo(camera);
    }

    public void TakePictures(Camera camera) => Console.WriteLine("Камера уже в режиме фото");

    //можно выключить камеру
    public void TurnOff(Camera camera)
    {
        Console.WriteLine("Нажата кнопка выключения");
        camera.SetState(new OffState());
    }

    public void TurnOn(Camera camera) => Console.WriteLine("Камера уже включена");
}

class VideoState : ICameraState
{
    public void RecordVideo(Camera camera) => Console.WriteLine("Камера уже в режиме видео");

    public void TakePictures(Camera camera)
    {
        Console.WriteLine("Переходим в режим фото");
        camera.TakePictures(camera);
    }

    //можно выключить камеру
    public void TurnOff(Camera camera)
    {
        Console.WriteLine("Нажата кнопка выключения");
        camera.SetState(new OffState());
    }

    public void TurnOn(Camera camera) => Console.WriteLine("Камера уже включена");
}

class OnState : ICameraState
{
    public void RecordVideo(Camera camera)
    {
        Console.WriteLine("Переходим в режим видео");
        camera.RecordVideo(camera);
    }

    public void TakePictures(Camera camera)
    {
        Console.WriteLine("Переходим в режим фото");
        camera.TakePictures(camera);
    }

    //можно выключить камеру
    public void TurnOff(Camera camera)
    {
        Console.WriteLine("Нажата кнопка выключения");
        camera.SetState(new OffState());
    }

    public void TurnOn(Camera camera) => Console.WriteLine("Камера уже включена");
}

class OffState : ICameraState
{
    public void RecordVideo(Camera camera) => Console.WriteLine("Камера ещё вылючена");
    public void TakePictures(Camera camera) => Console.WriteLine("Камера ещё вылючена");
    public void TurnOff(Camera camera) => Console.WriteLine("Камера уже вылючена");
    public void TurnOn(Camera camera)
    {
        Console.WriteLine("Нажата кнопка включения");
        camera.SetState(new OnState());
    }
}

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("SimpleState.Something");
        var sv = new Variable();
        sv.True();
        Console.WriteLine(sv);
        sv.False(); Console.WriteLine(sv);
        sv.False(); Console.WriteLine(sv);
        sv.False(); Console.WriteLine(sv);
        sv.True(); Console.WriteLine(sv);
    }
}