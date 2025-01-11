import React from 'react';
import styled from 'styled-components';

const ViewComponent = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
  background: ${props => props.theme.colors.background};
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  
  @media (max-width: 768px) {
    padding: 1rem;
    margin-bottom: 1rem;
  }

  @media (max-width: 480px) {
    padding: 0.75rem;
    border-radius: 6px;
  }
`;

export default ViewComponent; 