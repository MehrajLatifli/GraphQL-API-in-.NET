using GraphQLDemo.API.Models;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace GraphQLDemo.API.Schemas
{
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourceCreate([EventMessage] CourseResult course) => course;

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseUpdate(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
        {
            string topicName = $"{courseId}_{nameof(Subscription.CourseUpdate)}";
            return topicEventReceiver.SubscribeAsync<CourseResult>(topicName);
        }

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseDelete(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
        {
            string topicName = $"{courseId}_{nameof(CourseDelete)}";
            return topicEventReceiver.SubscribeAsync<CourseResult>(topicName);
        }

    }
}
