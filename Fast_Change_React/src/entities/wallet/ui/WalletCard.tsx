import type {
  WalletDto,
} from "../model/dto";

interface Props {
  wallet: WalletDto;
}

export function WalletCard({
  wallet,
}: Props) {
  return (
    <div
      className="
        rounded-3xl
        border
        border-exchange-border
        bg-exchange-card
        p-5
      "
    >
      <div
        className="
          flex
          justify-between
        "
      >
        <span className="
          text-sm
          text-exchange-muted
        ">
          {wallet.currency}
        </span>
      </div>

      <div
        className="
          mt-3
          text-2xl
          font-bold
        "
      >
        {wallet.balance}
      </div>
    </div>
  );
}