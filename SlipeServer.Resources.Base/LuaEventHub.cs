using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Mappers;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Services;
using System.Linq.Expressions;

namespace SlipeServer.Resources.Base;

public interface ILuaEventHub<THub>
{
    void Broadcast(Expression<Action<THub>> expression, Element? source = null);
    void Invoke(Player player, Expression<Action<THub>> expression, Element? source = null);
    void Invoke(IEnumerable<Player> players, Expression<Action<THub>> expression, Element? source = null);
}

internal class LuaEventHub<THub, TResource> : ILuaEventHub<THub> where TResource : Resource
{
    private readonly TResource _resource;
    private readonly LuaEventService _luaEventService;
    private readonly LuaValueMapper _luaValueMapper;
    private readonly RootElement _rootElement;

    public LuaEventHub(MtaServer server, LuaEventService luaEventService, LuaValueMapper luaValueMapper, RootElement rootElement)
    {
        _resource = server.GetAdditionalResource<TResource>();
        _luaEventService = luaEventService;
        _luaValueMapper = luaValueMapper;
        _rootElement = rootElement;
    }

    public void Invoke(Player player, Expression<Action<THub>> expression, Element? source = null)
    {
        var (eventName, values) = ConvertExpression(expression);
        _luaEventService.TriggerEventFor(player, eventName, source ?? player, values.ToArray());
    }

    public void Invoke(IEnumerable<Player> players, Expression<Action<THub>> expression, Element? source = null)
    {
        var (eventName, values) = ConvertExpression(expression);
        foreach (var player in players)
        {
            _luaEventService.TriggerEventFor(player, eventName, source ?? player, values.ToArray());
        }
    }

    public void Broadcast(Expression<Action<THub>> expression, Element? source = null)
    {
        var (eventName, values) = ConvertExpression(expression, "internalHubBroadcast");
        _luaEventService.TriggerEvent(eventName, source ?? _rootElement, values.ToArray());
    }

    private (string, IEnumerable<LuaValue>) ConvertExpression(Expression<Action<THub>> expression, string baseName = "internalHub")
    {
        if (expression is not LambdaExpression lambdaExpression)
            throw new NotSupportedException();

        if (lambdaExpression.Body is not MethodCallExpression methodCallExpression)
            throw new NotSupportedException();

        var luaValues = methodCallExpression.Arguments.Select(ConvertExpressionToLuaValue);
        var methodName = methodCallExpression.Method.Name;
        var eventName = $"{baseName}{_resource.Name}{methodName}";
        return (eventName, luaValues);
    }

    private LuaValue ConvertExpressionToLuaValue(Expression expression)
    {
        switch (expression)
        {
            case ConstantExpression constantExpression:
                return _luaValueMapper.Map(constantExpression.Value);
            case MemberExpression memberExpression:
                var value = Expression.Lambda(memberExpression).Compile().DynamicInvoke();
                return _luaValueMapper.Map(value);
            case NewExpression newExpression:
                var instantiate = Expression.Lambda<Func<LuaValue>>(newExpression).Compile();
                return instantiate();
            case UnaryExpression unaryExpression:
                var operandValue = Expression.Lambda(unaryExpression.Operand).Compile().DynamicInvoke();
                var result = Expression.Lambda(unaryExpression).Compile().DynamicInvoke();
                return _luaValueMapper.Map(result);
            case MethodCallExpression methodCallExpression when methodCallExpression.Arguments.Count == 0:
                var instanceValue = Expression.Lambda(methodCallExpression.Object).Compile().DynamicInvoke();
                var resultValue = methodCallExpression.Method.Invoke(instanceValue, null);
                return _luaValueMapper.Map(resultValue);
            default:
                throw new NotSupportedException(expression.ToString());
        }
    }
}
