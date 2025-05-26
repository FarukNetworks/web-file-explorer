// Helper function to format date
export function formatDate(date) {
  const now = new Date();
  const target = new Date(date);
  const diffTime = Math.abs(now - target);
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

  if (diffDays === 1) {
    return `Today, ${target.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    })}`;
  } else if (diffDays === 2) {
    return `Yesterday, ${target.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    })}`;
  } else {
    return target.toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }
} 