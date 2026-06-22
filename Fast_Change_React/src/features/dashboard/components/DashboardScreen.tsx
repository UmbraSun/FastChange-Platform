import React from 'react';

const stats = [
  { id: 1, label: 'Active Users', value: '12,400' },
  { id: 2, label: 'New Signups', value: '890' },
  { id: 3, label: 'Revenue', value: '$42,300' },
  { id: 4, label: 'Open Tickets', value: '27' },
];

const DashboardScreen: React.FC = () => {
  return (
    <div style={containerStyle}>
      <header style={headerStyle}>
        <div>
          <h1 style={titleStyle}>Dashboard</h1>
          <p style={subtitleStyle}>Overview of your platform performance and recent activity.</p>
        </div>
      </header>

      <section style={statsGridStyle}>
        {stats.map((stat) => (
          <div key={stat.id} style={statCardStyle}>
            <span style={statLabelStyle}>{stat.label}</span>
            <strong style={statValueStyle}>{stat.value}</strong>
          </div>
        ))}
      </section>

      <section style={sectionStyle}>
        <div style={panelStyle}>
          <h2 style={panelTitleStyle}>Recent Activity</h2>
          <ul style={activityListStyle}>
            <li>Payment processed for order #8231</li>
            <li>New account created by Anna Lee</li>
            <li>Server maintenance completed</li>
            <li>Support ticket #214 marked as resolved</li>
          </ul>
        </div>

        <div style={panelStyle}>
          <h2 style={panelTitleStyle}>Quick Insights</h2>
          <div style={insightRowStyle}>
            <div style={insightCardStyle}>
              <span style={insightLabelStyle}>Conversion</span>
              <strong>5.8%</strong>
            </div>
            <div style={insightCardStyle}>
              <span style={insightLabelStyle}>Bounce Rate</span>
              <strong>32%</strong>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

const containerStyle: React.CSSProperties = {
  padding: '24px',
  fontFamily: 'sans-serif',
  color: '#1f2937',
  backgroundColor: '#f8fafc',
  minHeight: '100vh',
};

const headerStyle: React.CSSProperties = {
  marginBottom: '24px',
};

const titleStyle: React.CSSProperties = {
  margin: 0,
  fontSize: '2rem',
};

const subtitleStyle: React.CSSProperties = {
  marginTop: '8px',
  color: '#475569',
};

const statsGridStyle: React.CSSProperties = {
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fit, minmax(180px, 1fr))',
  gap: '16px',
  marginBottom: '24px',
};

const statCardStyle: React.CSSProperties = {
  backgroundColor: '#ffffff',
  borderRadius: '12px',
  padding: '20px',
  boxShadow: '0 1px 3px rgba(15, 23, 42, 0.08)',
};

const statLabelStyle: React.CSSProperties = {
  display: 'block',
  marginBottom: '8px',
  color: '#64748b',
};

const statValueStyle: React.CSSProperties = {
  fontSize: '1.6rem',
};

const sectionStyle: React.CSSProperties = {
  display: 'grid',
  gridTemplateColumns: '1fr 1fr',
  gap: '16px',
};

const panelStyle: React.CSSProperties = {
  backgroundColor: '#ffffff',
  borderRadius: '12px',
  padding: '20px',
  boxShadow: '0 1px 3px rgba(15, 23, 42, 0.08)',
};

const panelTitleStyle: React.CSSProperties = {
  margin: '0 0 16px',
  fontSize: '1.1rem',
};

const activityListStyle: React.CSSProperties = {
  listStyle: 'none',
  margin: 0,
  padding: 0,
  color: '#334155',
};

const insightRowStyle: React.CSSProperties = {
  display: 'grid',
  gridTemplateColumns: '1fr 1fr',
  gap: '12px',
};

const insightCardStyle: React.CSSProperties = {
  backgroundColor: '#f1f5f9',
  borderRadius: '10px',
  padding: '14px',
};

const insightLabelStyle: React.CSSProperties = {
  display: 'block',
  marginBottom: '8px',
  color: '#475569',
};

export default DashboardScreen;
