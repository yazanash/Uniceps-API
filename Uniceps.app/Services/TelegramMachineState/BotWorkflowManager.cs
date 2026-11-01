using Uniceps.Entityframework.Models;

namespace Uniceps.app.Services.TelegramMachineState
{
    public class BotWorkflowManager
    {
        private readonly TelegramUserState _userState;
        public BotWorkflowManager(TelegramUserState userState)
        {
            _userState = userState;
        }
        private readonly Dictionary<(BotStep CurrentState, BotTrigger Trigger), BotStep> _transitions = new()
        {
            { (BotStep.Start, BotTrigger.Start), BotStep.WaitingEmail },
            { (BotStep.WaitingEmail, BotTrigger.EmailSubmitted), BotStep.ChoosingPlan },
            { (BotStep.ChoosingPlan, BotTrigger.PlanSelected), BotStep.WaitingCoupon },
            { (BotStep.WaitingCoupon, BotTrigger.CouponSubmitted), BotStep.WaitingCheckoutConfirmation },
            { (BotStep.WaitingCoupon, BotTrigger.CouponSkipped), BotStep.WaitingCheckoutConfirmation },

            { (BotStep.WaitingCheckoutConfirmation, BotTrigger.ConfirmCheckout), BotStep.ChoosingGateway },
            { (BotStep.ChoosingGateway, BotTrigger.GatewaySelected), BotStep.WaitingTransferCode },
            { (BotStep.WaitingTransferCode, BotTrigger.TransferCodeSubmitted), BotStep.WaitingConfirmation },
            { (BotStep.WaitingConfirmation, BotTrigger.ConfirmPayment), BotStep.Start }
        };
        public bool CanFire(BotTrigger trigger)
        {
            if (trigger == BotTrigger.Cancel) return true; 
            return _transitions.ContainsKey((_userState.Step, trigger));
        }
        public bool Fire(BotTrigger trigger)
        {
            if (trigger == BotTrigger.Cancel)
            {
                _userState.Step = BotStep.Start;
                return true;
            }

            var key = (_userState.Step, trigger);
            if (_transitions.TryGetValue(key, out var nextState))
            {
                _userState.Step = nextState;
                return true;
            }

            return false;
        }
    }
}
