export interface PortfolioPerformanceResponse {
  currency: string;
  currentValue: number;
  previousValue: number;
  changeAmount: number;
  changePercent: number;
}